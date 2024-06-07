using ChillDe.FMS.Services.Models.SkillModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
    }
}