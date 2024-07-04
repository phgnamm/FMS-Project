using ChillDe.FMS.Repositories.ViewModels.FreelancerModels;
using ChillDe.FMS.Services;
using ChillDe.FMS.Services.Interfaces;
using ChillDe.FMS.Services.Models.ProjectApplyModels;
using ChillDe.FMS.Services.Models.ProjectModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Interfaces;
using Services.Services;

namespace ChillDe.FMS.API.Controllers
{
    [Route("api/v1/project-apply")]
    [ApiController]
    public class ProjectApplyController : ControllerBase
    {
        private readonly IProjectApplyService _projectApplyService;

        public ProjectApplyController(IProjectApplyService projectApplyService)
        {
            _projectApplyService = projectApplyService;
        }

        //[HttpPost("{projectId}")]
        //public async Task<IActionResult> ImportRangeProjectApply(Guid projectApplyId, Guid projectId)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return ValidationProblem(ModelState);
        //        }
        //        var result = await _projectApplyService.ApplyFreelancer(projectApplyId, projectId);
        //        if (result.Status)
        //        {
        //            return Ok(result);
        //        }
        //        return BadRequest(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpPost()]
        [Authorize(Roles = "Administrator, Staff, Freelancer")]
        public async Task<IActionResult> CreateProjectApply(ProjectApplyCreateModel projectApplyModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationProblem(ModelState);
                }
                var result = await _projectApplyService.AddProjectApply(projectApplyModel);
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

        [HttpPut()]
        [Authorize(Roles = "Administrator, Staff, Freelancer")]
        public async Task<IActionResult> UpdateProjectApply(ProjectApplyUpdateModel projectApplyUpdateModel)
        {
            try
            {
                var result = await _projectApplyService.UpdateProjectApply(projectApplyUpdateModel);

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

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator, Staff, Freelancer")]
        public async Task<IActionResult> DeleteProjectApply(Guid id)
        {
            try
            {
                var result = await _projectApplyService.DeleteProjectApply(id);
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
        [Authorize(Roles = "Administrator, Staff, Freelancer")]
        public async Task<IActionResult> GetProjectApplyByFilter([FromQuery] ProjectApplyFilterModel projectApplyFilterModel)
        {
            try
            {
                var result = await _projectApplyService.GetProjectAppliesByFilter(projectApplyFilterModel);
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
    }
}
