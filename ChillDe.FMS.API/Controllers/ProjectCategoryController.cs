using ChillDe.FMS.Repositories.ViewModels.AccountModels;
using ChillDe.FMS.Services;
using ChillDe.FMS.Services.Interfaces;
using ChillDe.FMS.Services.Models.ProjectCategoryModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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

        [HttpGet("get-by-name")]
        [Authorize(Roles = "Administrator, Staff, Freelancer")]
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
        [HttpGet]
        [Authorize(Roles = "Administrator, Staff, Freelancer")]
        public async Task<IActionResult> GetProjectCategoriesByFilterAsync([FromQuery] ProjectCategoryFilterModel filterModel)
        {

            try
            {
                var result = await _projectCategoryService.GetProjectCategoriesByFilterAsync(filterModel);
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
        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator, Staff")]
        public async Task<IActionResult> UpdateProjectCategoryAsync(Guid id, [FromBody] ProjectCategoryUpdateModel updateModel)
        {
            try
            {
                var result = await _projectCategoryService.UpdateProjectCategoryAsync(id, updateModel);
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
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Authorize(Roles = "Administrator, Staff")]
        public async Task<IActionResult> CreateProjetCategory(List<ProjectCategoryCreateModel> createModels)
        {
            try
            {
                var result = await _projectCategoryService.CreateProjectCategoyryAsync(createModels);
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
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator, Staff")]
        public async Task<IActionResult> BlockProjectCategoryAsync(Guid id)
        {
            try
            {
                var result = await _projectCategoryService.BlockProjectCategoryAsync(id);
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
                return BadRequest(ex.Message);
            }
        }
    }
}
            
