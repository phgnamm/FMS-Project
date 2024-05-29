using Microsoft.AspNetCore.Mvc;
using Repositories.ViewModels.FreelancerModels;
using Services.Interfaces;
using Services.Services;

namespace API.Controllers
{
    [Route("api/v1/freelancer")]
    [ApiController]
    public class FreelancerController : ControllerBase
    {
        private readonly IFreelancerService _freelancerService;

        public FreelancerController(IFreelancerService freelancerService)
        {
            _freelancerService = freelancerService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFreelancer(Guid id)
        {
            try
            {
                var result = await _freelancerService.GetFreelancer(id);
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

        [HttpPost()]
        public async Task<IActionResult> ImportRangeFreelancer(List<FreelancerImportModel> freelancers)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationProblem(ModelState);
                }
                var result = await _freelancerService.AddRangeFreelancer(freelancers);
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFreelancer(Guid id, [FromBody] FreelancerImportModel freelancer)
        {
            try
            {
                var result = await _freelancerService.UpdateFreelancerAsync(id, freelancer);

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
