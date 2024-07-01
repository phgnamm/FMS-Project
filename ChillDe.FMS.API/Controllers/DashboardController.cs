using ChillDe.FMS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChillDe.FMS.API.Controllers
{
    [Route("api/v1/dashboard")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        
        [Authorize(Roles = "Administrator")]
        [HttpGet("administrator")]
        public async Task<IActionResult> GetAdminDashboard()
        {
            try
            {
                var result = await _dashboardService.GetAdminDashboard();
                if (result.Status)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        
        [Authorize(Roles = "Staff")]
        [HttpGet("staff")]
        public async Task<IActionResult> GetStaffDashboard()
        {
            try
            {
                var result = await _dashboardService.GetAdminDashboard();
                if (result.Status)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
