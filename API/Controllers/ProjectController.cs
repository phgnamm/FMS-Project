﻿using Microsoft.AspNetCore.Mvc;
using Repositories.ViewModels.FreelancerModels;
using Repositories.ViewModels.ProjectModels;
using Services.Interfaces;
using Services.Services;

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
        public async Task<IActionResult> CreateProject(ProjectAddModel projectAddModel)
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
    }
}
