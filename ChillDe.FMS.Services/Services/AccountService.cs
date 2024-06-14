using AutoMapper;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Interfaces;
using ChillDe.FMS.Repositories.Utils;
using ChillDe.FMS.Repositories.ViewModels.AccountModels;
using ChillDe.FMS.Repositories.ViewModels.CommonModels;
using ChillDe.FMS.Repositories.ViewModels.ResponseModels;
using ChillDe.FMS.Repositories.ViewModels.TokenModels;
using Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ChillDe.FMS.Repositories.Common;
using ChillDe.FMS.Repositories.Enums;
using Microsoft.EntityFrameworkCore;
using AccountModel = ChillDe.FMS.Repositories.Models.AccountModels.AccountModel;
using Role = ChillDe.FMS.Repositories.Enums.Role;
using FirebaseAdmin.Auth;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.Text;

namespace ChillDe.FMS.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<Account> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IClaimsService _claimsService;
        private readonly IAccountRepository _accountRepository;

        public AccountService(UserManager<Account> userManager, IUnitOfWork unitOfWork, IMapper mapper,
            IConfiguration configuration, IEmailService emailService, IClaimsService claimsService,
            IAccountRepository accountRepository)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
            _emailService = emailService;
            _claimsService = claimsService;
            _accountRepository = accountRepository;
        }

        public async Task<ResponseModel> Register(AccountRegisterModel accountRegisterModel)
        {
            // Check if Email already exists
            var existedEmail = await _userManager.FindByEmailAsync(accountRegisterModel.Email);

            if (existedEmail != null)
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "Email already exists"
                };
            }

            // Check if Code already exists
            var existedCode = await _unitOfWork.AccountRepository.GetAccountByCode(accountRegisterModel.Code);

            if (existedCode != null)
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "Code already exists"
                };
            }

            // Create new Account
            var user = _mapper.Map<Account>(accountRegisterModel);
            user.UserName = user.Email;
            user.CreationDate = DateTime.UtcNow;
            user.CreatedBy = _claimsService.GetCurrentUserId;
            // user.VerificationCode = AuthenticationTools.GenerateVerificationCode(6);
            // user.VerificationCodeExpiryTime = DateTime.Now.AddMinutes(15);

            var result = await _userManager.CreateAsync(user, accountRegisterModel.Password);

            if (result.Succeeded)
            {
                // Email verification (Disable this function if Users are not required to verify their Email)
                // await SendVerificationEmail(user);

                await _userManager.AddToRoleAsync(user, accountRegisterModel.Role.ToString());

                return new ResponseModel
                {
                    Status = true,
                    // Message = "Account has been created successfully, please verify your Email",
                    Message = "Account has been created successfully",
                    EmailVerificationRequired = true
                };
            }

            return new ResponseModel
            {
                Status = false,
                Message = "Cannot create Account"
            };
        }

        private async Task SendVerificationEmail(Account account)
        {
            await _emailService.SendEmailAsync(account.Email, "Verify your Email",
                $"Your verification code is {account.VerificationCode}. The code will expire in 15 minutes.", true);
        }

        public async Task<ResponseDataModel<TokenModel>> Login(AccountLoginModel accountLoginModel)
        {
            var user = await _userManager.FindByNameAsync(accountLoginModel.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, accountLoginModel.Password))
            {
                var authClaims = new List<Claim>
                {
                    new Claim("userId", user.Id.ToString()),
                    new Claim("userEmail", user.Email.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                var userRoles = await _userManager.GetRolesAsync(user);

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                // Check if Refresh Token is expired, if so then update
                if (user.RefreshToken == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
                {
                    var refreshToken = TokenTools.GenerateRefreshToken();
                    _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"],
                        out int refreshTokenValidityInDays);

                    // Update User's Refresh Token
                    user.RefreshToken = refreshToken;
                    user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

                    var result = await _userManager.UpdateAsync(user);

                    if (!result.Succeeded)
                    {
                        return new ResponseDataModel<TokenModel>
                        {
                            Status = false,
                            Message = "Cannot login"
                        };
                    }
                }

                var jwtToken = TokenTools.CreateJWTToken(authClaims, _configuration);

                return new ResponseDataModel<TokenModel>
                {
                    Status = true,
                    Message = "Login successfully",
                    EmailVerificationRequired = !user.EmailConfirmed,
                    Data = new TokenModel
                    {
                        AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                        AccessTokenExpiryTime = jwtToken.ValidTo.ToLocalTime(),
                        RefreshToken = user.RefreshToken,
                    }
                };
            }

            return new ResponseDataModel<TokenModel>
            {
                Status = false,
                Message = "Cannot login"
            };
        }

        public async Task<ResponseDataModel<TokenModel>> RefreshToken(RefreshTokenModel refreshTokenModel,
            string refreshTokenFromClient)
        {
            // Validate Access Token and Refresh Token
            var principal = TokenTools.GetPrincipalFromExpiredToken(refreshTokenModel.AccessToken, _configuration);

            if (principal == null)
            {
                return new ResponseDataModel<TokenModel>
                {
                    Status = false,
                    Message = "Invalid Access Token or Refresh Token"
                };
            }

            var user = await _userManager.FindByIdAsync(principal.FindFirst("userId").Value);

            if (user == null || user.RefreshToken != refreshTokenFromClient ||
                user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return new ResponseDataModel<TokenModel>
                {
                    Status = false,
                    Message = "Invalid Access Token or Refresh Token"
                };
            }

            // Start to refresh Access Token and Refresh Token
            var refreshToken = TokenTools.GenerateRefreshToken();
            _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

            // Update User's Refresh Token
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return new ResponseDataModel<TokenModel>
                {
                    Status = false,
                    Message = "Cannot refresh the Token"
                };
            }

            var jwtToken = TokenTools.CreateJWTToken(principal.Claims.ToList(), _configuration);

            return new ResponseDataModel<TokenModel>
            {
                Status = true,
                Message = "Refresh Token successfully",
                Data = new TokenModel
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                    AccessTokenExpiryTime = jwtToken.ValidTo.ToLocalTime(),
                    RefreshToken = user.RefreshToken,
                }
            };
        }

        public async Task<ResponseModel> VerifyEmail(string email, string verificationCode)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "User not found"
                };
            }

            if (user.VerificationCodeExpiryTime < DateTime.UtcNow)
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "The code is expired",
                    EmailVerificationRequired = false
                };
            }

            if (user.VerificationCode == verificationCode)
            {
                user.EmailConfirmed = true;
                user.VerificationCode = null;
                user.VerificationCodeExpiryTime = null;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return new ResponseModel
                    {
                        Status = true,
                        Message = "Verify Email successfully",
                    };
                }
            }

            return new ResponseModel
            {
                Status = false,
                Message = "Cannot verify Email",
            };
        }

        public async Task<ResponseModel> ResendVerificationEmail(EmailModel emailModel)
        {
            var currentUserId = _claimsService.GetCurrentUserId;

            if (emailModel == null && currentUserId == null)
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "User not found",
                };
            }

            Account user = null;

            if (emailModel != null && currentUserId == null)
            {
                user = await _userManager.FindByEmailAsync(emailModel.Email);

                if (user == null)
                {
                    return new ResponseModel
                    {
                        Status = false,
                        Message = "User not found",
                    };
                }
            }
            else if (emailModel == null && currentUserId != null)
            {
                user = await _userManager.FindByIdAsync(currentUserId.ToString());
            }
            else if (emailModel != null && currentUserId != null)
            {
                user = await _userManager.FindByEmailAsync(emailModel.Email);

                if (user == null || user.Id != currentUserId)
                {
                    return new ResponseModel
                    {
                        Status = false,
                        Message = "Cannot resend Verification Email",
                        EmailVerificationRequired = true
                    };
                }
            }

            if (user.EmailConfirmed)
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "Email has been verified",
                };
            }

            // Update new Verification IdToken
            user.VerificationCode = AuthenticationTools.GenerateVerificationCode(6);
            user.VerificationCodeExpiryTime = DateTime.Now.AddMinutes(15);
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                await SendVerificationEmail(user);

                return new ResponseModel
                {
                    Status = true,
                    Message = "Resend Verification Email successfully",
                    EmailVerificationRequired = true
                };
            }

            return new ResponseModel
            {
                Status = false,
                Message = "Cannot resend Verification Email",
                EmailVerificationRequired = true
            };
        }

        public async Task<ResponseModel> ChangePassword(AccountChangePasswordModel accountChangePasswordModel)
        {
            var currentUserId = _claimsService.GetCurrentUserId;
            var user = await _userManager.FindByIdAsync(currentUserId.ToString());

            var result = await _userManager.ChangePasswordAsync(user, accountChangePasswordModel.OldPassword,
                accountChangePasswordModel.NewPassword);

            if (result.Succeeded)
            {
                return new ResponseModel
                {
                    Status = true,
                    Message = "Change Password successfully",
                };
            }

            return new ResponseModel
            {
                Status = false,
                Message = "Cannot change Password",
            };
        }

        public async Task<ResponseModel> ForgotPassword(EmailModel emailModel)
        {
            var user = await _userManager.FindByEmailAsync(emailModel.Email);

            if (user == null)
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "User not found",
                };
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            // todo modify this Email body to send a URL redirect to the frontend page and contain the token as a parameter in the URL
            await _emailService.SendEmailAsync(user.Email, "Reset your Password",
                $"Your token is {token}. The token will expire in 15 minutes.", true);

            return new ResponseModel
            {
                Status = true,
                Message = "An Email has been sent, please check your inbox",
            };
        }

        public async Task<ResponseModel> ResetPassword(AccountResetPasswordModel accountResetPasswordModel)
        {
            var user = await _userManager.FindByEmailAsync(accountResetPasswordModel.Email);

            if (user == null)
            {
                return new ResponseModel
                {
                    Status = false,
                    Message = "User not found",
                };
            }

            var result = await _userManager.ResetPasswordAsync(user, accountResetPasswordModel.Token,
                accountResetPasswordModel.Password);

            if (result.Succeeded)
            {
                return new ResponseModel
                {
                    Status = true,
                    Message = "Reset Password successfully",
                };
            }

            return new ResponseModel
            {
                Status = false,
                Message = "Cannot reset Password",
            };
        }

        public bool IsGoogleIdToken(string idToken)
        {
            var parts = idToken.Split('.');
            if (parts.Length != 3)
            {
                return false;
            }
            var payload = parts[1];
            var decodedPayload = Base64UrlEncoder.DecodeBytes(payload);
            if (decodedPayload == null)
            {
                return false;
            }
            var payloadJson = Encoding.UTF8.GetString(decodedPayload);
            var jsonObject = JObject.Parse(payloadJson);
            var audience = jsonObject["aud"]?.ToString();
            var googleClientId = _configuration["OAuth2:Google:ClientId"];
            if (audience == googleClientId)
            {
                return true;
            }
            return false;
        }

        public async Task<ResponseDataModel<TokenModel>> LoginGoogle(LoginGoogleIdTokenModel loginGoogleIdTokenModel)
        {
            FirebaseToken decodedToken = null;
            GoogleJsonWebSignature.Payload payload = null;
            var validToken = false;

            var isGoogleIdToken = IsGoogleIdToken(loginGoogleIdTokenModel.IdToken);

            if (isGoogleIdToken)
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new List<string> { _configuration["OAuth2:Google:ClientId"] }
                };

                payload = await GoogleJsonWebSignature.ValidateAsync(loginGoogleIdTokenModel.IdToken, settings);
                validToken = true;
            }
            else
            {
                decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(loginGoogleIdTokenModel.IdToken);
                validToken = true;
            }

            if (!validToken)
            {
                return new ResponseDataModel<TokenModel>
                {
                    Status = false,
                    Message = "Invalid credentials"
                };
            }
            var email = payload != null ? payload.Email : decodedToken.Claims["email"].ToString();
            var user = await _unitOfWork.FreelancerRepository.GetFreelancerByEmail(email);
            if (user == null)
            {
                return new ResponseDataModel<TokenModel>
                {
                    Status = false,
                    Message = "User not found"
                };
            }

            // JWT Token
            var authClaims = new List<Claim>
            {
                new Claim("userId", user.Id.ToString()),
                new Claim("userEmail", user.Email.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            authClaims.Add(new Claim(ClaimTypes.Role, "Freelancer"));

            // Check if Refresh Token is expired, if so then update
            if (user.RefreshToken == null || user.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                var refreshToken = TokenTools.GenerateRefreshToken();
                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

                // Update User's Refresh Token
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

                _unitOfWork.FreelancerRepository.Update(user);

                if (await _unitOfWork.SaveChangeAsync() > 0)
                {
                    return new ResponseDataModel<TokenModel>
                    {
                        Status = false,
                        Message = "Cannot login"
                    };
                }
            }

            var jwtToken = TokenTools.CreateJWTToken(authClaims, _configuration);

            return new ResponseDataModel<TokenModel>
            {
                Status = true,
                Message = "Login successfully",
                // EmailVerificationRequired = !user.EmailConfirmed,
                Data = new TokenModel
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                    AccessTokenExpiryTime = jwtToken.ValidTo.ToLocalTime(),
                    RefreshToken = user.RefreshToken,
                }
            };
        }

        public async Task<ResponseDataModel<AccountModel>> GetAccount(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return new ResponseDataModel<AccountModel>()
                {
                    Status = false,
                    Message = "User not found"
                };
            }

            var userModel = _mapper.Map<AccountModel>(user);
            var role = await _userManager.GetRolesAsync(user);
            userModel.Role = Enum.Parse(typeof(Role), role[0]).ToString();

            return new ResponseDataModel<AccountModel>()
            {
                Status = true,
                Message = "Get account successfully",
                Data = userModel
            };
        }

        public async Task<Pagination<AccountModel>> GetAllAccounts(AccountFilterModel accountFilterModel)
        {
            var accountList = await _unitOfWork.AccountRepository.GetAllAsync(pageIndex: accountFilterModel.PageIndex,
                pageSize: accountFilterModel.PageSize,
                filter: (x =>
                    x.IsDeleted == accountFilterModel.IsDeleted &&
                    (accountFilterModel.Gender == null || x.Gender == accountFilterModel.Gender) &&
                    (accountFilterModel.Role == null || x.Role == accountFilterModel.Role.ToString()) &&
                    (string.IsNullOrEmpty(accountFilterModel.Search) ||
                     x.FirstName.ToLower().Contains(accountFilterModel.Search.ToLower()) ||
                     x.LastName.ToLower().Contains(accountFilterModel.Search.ToLower()) ||
                     x.Code.ToLower().Contains(accountFilterModel.Search.ToLower()) ||
                     x.Email.ToLower().Contains(accountFilterModel.Search.ToLower()))),
                orderBy: (x =>
                {
                    switch (accountFilterModel.Order.ToLower())
                    {
                        case "first-name":
                            return accountFilterModel.OrderByDescending
                                ? x.OrderByDescending(x => x.FirstName)
                                : x.OrderBy(x => x.FirstName);
                        case "last-name":
                            return accountFilterModel.OrderByDescending
                                ? x.OrderByDescending(x => x.LastName)
                                : x.OrderBy(x => x.LastName);
                        case "code":
                            return accountFilterModel.OrderByDescending
                                ? x.OrderByDescending(x => x.Code)
                                : x.OrderBy(x => x.Code);
                        case "date-of-birth":
                            return accountFilterModel.OrderByDescending
                                ? x.OrderByDescending(x => x.DateOfBirth)
                                : x.OrderBy(x => x.DateOfBirth);
                        default:
                            return accountFilterModel.OrderByDescending
                                ? x.OrderByDescending(x => x.CreationDate)
                                : x.OrderBy(x => x.CreationDate);
                    }
                })
            );

            if (accountList != null)
            {
                var accountModelList = _mapper.Map<List<AccountModel>>(accountList.Data);
                return new Pagination<AccountModel>(accountModelList, accountList.TotalCount, accountFilterModel.PageIndex,
                    accountFilterModel.PageSize);
            }

            return null;
        }

        public async Task<ResponseModel> UpdateAccount(AccountUpdateModel accountUpdateModel, Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return new ResponseModel()
                {
                    Status = false,
                    Message = "User not found"
                };
            }

            // Check if Email already exists
            if (accountUpdateModel.Email != user.Email)
            {
                var existedEmail = await _userManager.FindByEmailAsync(accountUpdateModel.Email);

                if (existedEmail != null)
                {
                    return new ResponseModel
                    {
                        Status = false,
                        Message = "Email already exists"
                    };
                }
            }

            // Check if Code already exists
            if (accountUpdateModel.Code != user.Code)
            {
                var existedCode = await _unitOfWork.AccountRepository.GetAccountByCode(accountUpdateModel.Code);

                if (existedCode != null)
                {
                    return new ResponseModel
                    {
                        Status = false,
                        Message = "Code already exists"
                    };
                }
            }

            var role = await _userManager.GetRolesAsync(user);
            if (accountUpdateModel.Role.ToString() != role[0])
            {
                // Check if old role is Staff and he/she is working on a project
                if (role[0] == Role.Staff.ToString())
                {
                    var projectList = await _unitOfWork.ProjectRepository.GetProjectByAccount(id, false,
                        [ProjectStatus.Pending, ProjectStatus.Processing, ProjectStatus.Checking]);

                    if (projectList.Count > 0)
                    {
                        return new ResponseModel()
                        {
                            Status = false,
                            Message = "Can not change role because he/she is working on a project"
                        };
                    }
                }

                await _userManager.RemoveFromRoleAsync(user, role[0]);
                await _userManager.AddToRoleAsync(user, accountUpdateModel.Role.ToString());
            }

            user.FirstName = accountUpdateModel.FirstName;
            user.LastName = accountUpdateModel.LastName;
            user.Gender = accountUpdateModel.Gender;
            user.DateOfBirth = accountUpdateModel.DateOfBirth;
            user.PhoneNumber = accountUpdateModel.PhoneNumber;
            user.Email = accountUpdateModel.Email;
            user.Code = accountUpdateModel.Code;
            user.ModificationDate = DateTime.UtcNow;
            user.ModifiedBy = _claimsService.GetCurrentUserId;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return new ResponseModel
                {
                    Status = true,
                    Message = "Update Account successfully",
                };
            }

            return new ResponseModel
            {
                Status = false,
                Message = "Cannot update Account",
            };
        }

        public async Task<ResponseModel> DeleteAccount(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return new ResponseModel()
                {
                    Status = false,
                    Message = "User not found"
                };
            }

            var projectList = await _unitOfWork.ProjectRepository.GetProjectByAccount(id, false,
                [ProjectStatus.Pending, ProjectStatus.Processing, ProjectStatus.Checking]);

            if (projectList.Count > 0)
            {
                return new ResponseModel()
                {
                    Status = false,
                    Message = "Can not delete this Account because he/she is working on a project"
                };
            }

            user.IsDeleted = true;
            user.DeletionDate = DateTime.UtcNow;
            user.DeletedBy = _claimsService.GetCurrentUserId;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return new ResponseModel
                {
                    Status = true,
                    Message = "Delete Account successfully",
                };
            }

            return new ResponseModel
            {
                Status = false,
                Message = "Cannot delete Account",
            };
        }

        public async Task<ResponseModel> RestoreAccount(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                return new ResponseModel()
                {
                    Status = false,
                    Message = "User not found"
                };
            }
            
            user.IsDeleted = false;
            user.DeletionDate = null;
            user.DeletedBy = null;
            user.ModificationDate = DateTime.UtcNow;
            user.ModifiedBy = _claimsService.GetCurrentUserId;
            
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return new ResponseModel
                {
                    Status = true,
                    Message = "Restore Account successfully",
                };
            }

            return new ResponseModel
            {
                Status = false,
                Message = "Cannot restore Account",
            };
        }
    }
}