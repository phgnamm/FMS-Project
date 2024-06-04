using Microsoft.AspNetCore.Mvc;
using Repositories.ViewModels.ProjectDeliverableModels;
using Repositories.ViewModels.ProjectModels;
using Services.Interfaces;

namespace API.Controllers
{
    [Route("api/v1/projectDeliverable")]
    [ApiController]
    public class ProjectDeliverableController : ControllerBase
    {
        private readonly IProjectDeliverableService _projectDeliverableService;

        public ProjectDeliverableController(IProjectDeliverableService projectDeliverableService)
        {
            _projectDeliverableService = projectDeliverableService;
        }

        [HttpPost]
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
    }
}
