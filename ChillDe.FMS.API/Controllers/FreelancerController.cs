using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ChillDe.FMS.Repositories.ViewModels.FreelancerModels;
using Microsoft.AspNetCore.Authorization;
using Services.Interfaces;


namespace ChillDe.FMS.API.Controllers
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
        [Authorize(Roles = "Administrator, Staff, Freelancer")]
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

        [HttpGet]
        [Authorize(Roles = "Administrator, Staff")]
        public async Task<IActionResult> GetFreelancersByFilter([FromQuery] FreelancerFilterModel freelancerFilterModel)
        {
            try
            {
                var result = await _freelancerService.GetFreelancersByFilter(freelancerFilterModel);
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

        [HttpPost()]
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator, Freelancer")]
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

        [HttpDelete()]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteFreelancer(List<Guid> ids)
        {
            try
            {
                var result = await _freelancerService.DeleteFreelancer(ids);
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

        [HttpPut("restore/{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> RestoreAccount(Guid id)
        {
            try
            {
                var result = await _freelancerService.RestoreFreelancer(id);
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