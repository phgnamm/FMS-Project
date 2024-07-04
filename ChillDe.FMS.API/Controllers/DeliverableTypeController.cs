using Azure;
using ChillDe.FMS.Repositories.Entities;
using ChillDe.FMS.Services.Interfaces;
using ChillDe.FMS.Services.Models.DeliverableTypeModels;
using ChillDe.FMS.Services.Models.ProjectModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ChillDe.FMS.API.Controllers
{
    [Route("api/v1/deliverable-type")]
    [ApiController]
    public class DeliverableTypeController : ControllerBase
    {
        private readonly IDeliverableTypeService _deliverableTypeService;

        public DeliverableTypeController(IDeliverableTypeService deliverableTypeService)
        {
            _deliverableTypeService = deliverableTypeService;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator, Staff")]
        public async Task<IActionResult> GetAllDeliverableType
            ([FromQuery] DeliverableTypeFilterModel deliverableTypeFilterModel)
        {
            try
            {
                var result = await _deliverableTypeService.GetAllDeliverableType(deliverableTypeFilterModel);
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

        [HttpPost]
        [Authorize(Roles = "Administrator, Staff")]
        public async Task<IActionResult> CreateDeliverableType(DeliverableTypeCreateModel deliverableTypeCreateModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationProblem(ModelState);
                }
                var result = await _deliverableTypeService.CreateDeliverableType(deliverableTypeCreateModel);
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
        [Authorize(Roles = "Administrator, Staff")]
        public async Task<IActionResult> UpdateDeliverableType(Guid id, [FromBody] DeliverableTypeCreateModel deliverableTypeCreateModel)
        {
            try
            {
                var result = await _deliverableTypeService.UpdateDeliverableType(id, deliverableTypeCreateModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator, Staff")]
        public async Task<IActionResult> DeleteDeliverableType(Guid id)
        {
            try
            {
                var result = await _deliverableTypeService.DeleteDeliverableType(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpGet("{id}")]
        [Authorize(Roles = "Administrator, Staff")]
        public async Task<IActionResult> GetDeliverableType(Guid id)
        {
            try
            {
                var result = await _deliverableTypeService.GetDeliverableType(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
