using ChillDe.FMS.Services.Models.SkillModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.DependencyResolver;
using Services.Interfaces;

namespace ChillDe.FMS.API.Controllers
{
    [Route("api/v1/skill")]
    [ApiController]
    public class SkillController : ControllerBase
    {
        private readonly ISkillService _skillService;

        public SkillController(ISkillService skillService)
        {
            _skillService = skillService;
        }

        [HttpGet("group-by-type")]
        // [Authorize(Roles = "Administrator, Staff, Freelancer")]
        public async Task<IActionResult> GetAllSkillsGroupByType([FromQuery] SkillFilterModel skillFilterModel)
        {
            try
            {
                var result = await _skillService.GetAllSkillsGroupByType(skillFilterModel);

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
        [HttpGet]
        public async Task<IActionResult> GetAllSkillsByFilter([FromQuery] SkillFilterModel skillFilterModel)
        {
            try
            {
                var result = await _skillService.GetAllSkill(skillFilterModel);

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
        public async Task<IActionResult> UpdateSkill(Guid id, [FromBody] SkillUpdateModel skillUpdateModel)
        {
            try
            {
                var result = await _skillService.UpdateSkill(id, skillUpdateModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSkill(Guid id)
        {
            try
            {
                var result = await _skillService.DeleteSkill(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> CreateSkillAsynce([FromBody] List<SkillCreateModel> skillCreateModels)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _skillService.CreateSkill(skillCreateModels);
                    if (result != null)
                    {
                        return Ok(result);
                    }
                }
                return BadRequest(ModelState);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSkill(Guid id)
        {
            try
            {
                var result = await _skillService.GetSkill(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}