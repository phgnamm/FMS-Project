using System.Security.Claims;
using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Repositories.Interfaces;
using ChillDe.FMS.Repositories.Utils;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace ChillDe.FMS.API.Middlewares;

public class AccountStatusMiddleware : IMiddleware
{
    private readonly ILogger<AccountStatusMiddleware> _logger;
    private readonly IConfiguration _configuration;
    private readonly UserManager<Account> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUnitOfWork _unitOfWork;

    public AccountStatusMiddleware(ILogger<AccountStatusMiddleware> logger, IConfiguration configuration,
        UserManager<Account> userManager, IHttpContextAccessor httpContextAccessor, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _configuration = configuration;
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _unitOfWork = unitOfWork;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var identity = _httpContextAccessor.HttpContext?.User?.Identity as ClaimsIdentity;
        var extractedId = AuthenticationTools.GetCurrentUserId(identity);

        if (extractedId != null)
        {
            var isFreelancer = _httpContextAccessor.HttpContext?.User?.IsInRole("Freelancer");

            // Check freelancer
            if (isFreelancer != null && isFreelancer == true)
            {
                var userAccount = await _unitOfWork.FreelancerRepository.GetAsync(Guid.Parse(extractedId.ToString()));
                if (userAccount != null && userAccount.IsDeleted == true)
                {
                    userAccount.RefreshToken = null;
                    userAccount.RefreshTokenExpiryTime = null;
                    _unitOfWork.FreelancerRepository.Update(userAccount);
                    await _unitOfWork.SaveChangeAsync();

                    var response = new
                    {
                        isBlocking = true,
                        message = "Account is banned"
                    };

                    var jsonResponse = JsonConvert.SerializeObject(response);
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(jsonResponse);

                    return;
                }
            }
            
            // Check other account roles
            else if (isFreelancer != null && isFreelancer == false)
            {
                var userAccount = await _userManager.FindByIdAsync(extractedId.ToString());
                if (userAccount != null && userAccount.IsDeleted == true)
                {
                    userAccount.RefreshToken = null;
                    userAccount.RefreshTokenExpiryTime = null;
                    await _userManager.UpdateAsync(userAccount);

                    var response = new
                    {
                        isBlocking = true,
                        message = "Account is banned"
                    };

                    var jsonResponse = JsonConvert.SerializeObject(response);
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(jsonResponse);

                    return;
                }
            }
        }

        await next(context);
    }
}