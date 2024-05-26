using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Repositories.Common;
using Repositories.ViewModels.AccountModels;
using Services.Interfaces;

namespace API.Controllers
{
    [Route("api/v1/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccount(Guid id)
        {
            try
            {
                var result = await _accountService.GetAccount(id);
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
        public async Task<IActionResult> GetAccountByFilters([FromQuery] PaginationParameter paginationParameter,
            [FromQuery] AccountFilterModel accountFilterModel)
        {
            try
            {
                var result = await _accountService.GetAccountsByFilter(paginationParameter, accountFilterModel);
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
    }
}