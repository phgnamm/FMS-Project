using ChillDe.FMS.Services.Interfaces;
using ChillDe.FMS.Services.Models.TransactionModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ChillDe.FMS.API.Controllers
{
    [Route("api/v1/transaction")]
    [ApiController]
    public class TransactionController:ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactionsByFilter([FromQuery] TransactionFilterModel transactionFilterModel)
        {
            try
            {
                var result = await _transactionService.GetTransactionByFilter(transactionFilterModel);
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
        [HttpPost("submit-project/{projectApplyId}")]
        public async Task<IActionResult> SubmitProject(Guid projectApplyId)
        {
            try
            {
                var result = await _transactionService.SubmitProject(projectApplyId);

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
