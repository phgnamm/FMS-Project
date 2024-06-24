﻿using ChillDe.FMS.Repositories.Enums;
using ChillDe.FMS.Services.Interfaces;
using ChillDe.FMS.Services.Models.DeliverableProductModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ChillDe.FMS.API.Controllers
{
    [Route("api/v1/deliverable-product")]
    [ApiController]
    public class DeliverableProductController : ControllerBase
    {
        private readonly IDeliverableProductService _deliverableProductService;

        public DeliverableProductController(IDeliverableProductService deliverableProductService)
        {
            _deliverableProductService = deliverableProductService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDeliverableProduct
            (DeliverableProductCreateModel deliverableProductCreateModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return ValidationProblem(ModelState);
                }
                var result = await _deliverableProductService.CreateDeliverableProduct(deliverableProductCreateModel);
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

        [HttpDelete]
        public async Task<IActionResult> DeleteDeliverableProduct(Guid id)
        {
            try
            {
                var result = await _deliverableProductService.DeleteDeliverableProduct(id);
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

        [HttpPut]
        public async Task<IActionResult> UpdateProject
            (Guid deliverableProductId, DeliverableProductStatus status)
        {
            try
            {
                var result = await _deliverableProductService.UpdateDeliverableProduct
                    (deliverableProductId, status);

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
        public async Task<IActionResult> GetDeliverableProductByFilter
            ([FromQuery] DeliverableProductFilterModel deliverableProductFilterModel)
        {
            try
            {
                var result = await _deliverableProductService
                    .GetAllDeliverableProduct(deliverableProductFilterModel);
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