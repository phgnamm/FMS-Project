using ChillDe.FMS.Repositories.Interfaces;
using ChillDe.FMS.Repositories.Utils;
using System.Security.Claims;

namespace ChillDe.FMS.API.Services
{
    public class ClaimsService : IClaimsService
    {
		public Guid? GetCurrentUserId { get; }

		public ClaimsService(IHttpContextAccessor httpContextAccessor)
        {
            var identity = httpContextAccessor.HttpContext?.User?.Identity as ClaimsIdentity;
			GetCurrentUserId = AuthenticationTools.GetCurrentUserId(identity);
        }
    }
}
