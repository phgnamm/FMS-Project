using ChillDe.FMS.Services.Models.ProjectDeliverableModel;
using ChillDe.FMS.Services.Models.ProjectModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Interfaces;
using Services.Services;

namespace API.Controllers
{
    [Route("api/v1/project-deliverable")]
    [ApiController]
    public class ProjectDeliverableController : ControllerBase
    {
        private readonly IProjectDeliverableService _projectDeliverableService;

        public ProjectDeliverableController(IProjectDeliverableService projectDeliverableService)
        {
            _projectDeliverableService = projectDeliverableService;
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Staff")]
        public async Task<IActionResult> CreateProjectDeliverable
            (ProjectDeliverableCreateModel projectDeliverableModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationProblem(ModelState);
                }
                var result = await _projectDeliverableService.CreateProjectDeliverable(projectDeliverableModel);
                if (result.Status)
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrator, Staff, Freelancer")]
        public async Task<IActionResult> GetProjectDeliverableByFilter
            ([FromQuery] ProjectDeliverableFilterModel projectDeliverableFilterModel)
        {
            try
            {
                var result = await _projectDeliverableService.GetAllProjectDeliverable(projectDeliverableFilterModel);
                var metadata = new
                {
                    result.PageSize,
                    result.CurrentPage,
                    result.TotalPages,
                };

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Administrator, Staff")]
        public async Task<IActionResult> DeleteProjectDeliverable(Guid id)
        {
            try
            {
                var result = await _projectDeliverableService.DeleteProjectDeliverable(id);
                if (result.Status)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
