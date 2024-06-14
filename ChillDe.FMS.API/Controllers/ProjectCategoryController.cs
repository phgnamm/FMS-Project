using ChillDe.FMS.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.Services;

namespace ChillDe.FMS.API.Controllers
{
    [Route("api/v1/project-category")]
    [ApiController]
    public class ProjectCategoryController : ControllerBase
    {
        private readonly IProjectCategoryService _projectCategoryService;

        public ProjectCategoryController(IProjectCategoryService projectCategoryService)
        {
            _projectCategoryService = projectCategoryService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetProjectCategoriesByNames([FromQuery] List<string> names)
        {
            try
            {
                var result = await _projectCategoryService.GetProjectCategoriesByNames(names);
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
