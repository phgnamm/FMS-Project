using ChillDe.FMS.Repositories.Common;
using ChillDe.FMS.Repositories.ViewModels.AccountModels;
using ChillDe.FMS.Repositories.ViewModels.CommonModels;
using ChillDe.FMS.Repositories.ViewModels.ResponseModels;
using ChillDe.FMS.Repositories.ViewModels.TokenModels;
using AccountModel = ChillDe.FMS.Repositories.Models.AccountModels.AccountModel;

namespace Services.Interfaces
{
    public interface IAccountService
    {
        Task<ResponseModel> Register(AccountRegisterModel accountRegisterModel);
        Task<ResponseDataModel<TokenModel>> Login(AccountLoginModel accountLoginModel);

        Task<ResponseDataModel<TokenModel>> RefreshToken(RefreshTokenModel refreshTokenModel,
            string refreshTokenFromClient);

        Task<ResponseModel> VerifyEmail(string email, string verificationCode);
        Task<ResponseModel> ResendVerificationEmail(EmailModel? emailModel);
        Task<ResponseModel> ChangePassword(AccountChangePasswordModel accountChangePasswordModel);
        Task<ResponseModel> ForgotPassword(EmailModel emailModel);
        Task<ResponseModel> ResetPassword(AccountResetPasswordModel accountResetPasswordModel);
        Task<ResponseDataModel<TokenModel>> LoginGoogle(LoginGoogleIdTokenModel loginGoogleIdTokenModel);
        Task<ResponseDataModel<AccountModel>> GetAccount(Guid id);

        Task<Pagination<AccountModel>> GetAllAccounts(AccountFilterModel accountFilterModel);
        Task<ResponseModel> UpdateAccount(AccountUpdateModel accountUpdateModel, Guid id);
        Task<ResponseModel> DeleteAccount(Guid id);
        Task<ResponseModel> RestoreAccount(Guid id);
    }
}