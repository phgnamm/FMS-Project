using ChillDe.FMS.Repositories.Enums;
using ChillDe.FMS.Services.Models.ProjectModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Interfaces;

namespace API.Controllers
{
    [Route("api/v1/project")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpPost()]
        public async Task<IActionResult> CreateProject(ProjectCreateModel projectAddModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationProblem(ModelState);
                }
                var result = await _projectService.CreateProject(projectAddModel);
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject(Guid id)
        {
            try
            {
                var result = await _projectService.GetProject(id);
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

        [HttpGet]
        public async Task<IActionResult> GetProjectByFilter([FromQuery] ProjectFilterModel projectFilterModel)
        {
            try
            {
                var result = await _projectService.GetAllProjects(projectFilterModel);
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

        [HttpPut("{projectId}")]
        public async Task<IActionResult> UpdateProject(Guid projectId, [FromBody] ProjectUpdateModel project)
        {
            try
            {
                var result = await _projectService.UpdateProject(projectId, project);

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

        [HttpDelete()]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            try
            {
                var result = await _projectService.DeleteProject(id);
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

        [HttpPut("close")]
        public async Task<IActionResult> CloseProject(Guid projectId, ProjectStatus status)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationProblem(ModelState);
                }
                var result = await _projectService.CloseProject(projectId, status);
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
