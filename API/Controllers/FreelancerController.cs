using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repositories.Common;
using Repositories.ViewModels.AccountModels;
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

        [HttpGet]
        public async Task<IActionResult> GetFreelancersByFilter([FromQuery] PaginationParameter paginationParameter,
            [FromQuery] FreelancerFilterModel freelancerFilterModel)
        {
            try
            {
                var result = await _freelancerService.GetFreelancersByFilter(paginationParameter, freelancerFilterModel);
                var metadata = new
                {
                    result.TotalCount,
                    result.PageSize,
                    result.CurrentPage,
                    result.TotalPages,
                    result.HasNext,
                    result.HasPrevious
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

        [HttpDelete()]
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
    }
}
