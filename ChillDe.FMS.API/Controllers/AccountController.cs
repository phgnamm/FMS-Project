using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ChillDe.FMS.Repositories.Common;
using ChillDe.FMS.Repositories.ViewModels.AccountModels;
using Services.Interfaces;

namespace ChillDe.FMS.API.Controllers
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
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetAllAccounts([FromQuery] AccountFilterModel accountFilterModel)
        {
            try
            {
                var result = await _accountService.GetAllAccounts(accountFilterModel);
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
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> UpdateAccount([FromBody] AccountUpdateModel accountUpdateModel, Guid id)
        {
            try
            {
                var result = await _accountService.UpdateAccount(accountUpdateModel, id);
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

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteAccount(Guid id)
        {
            try
            {
                var result = await _accountService.DeleteAccount(id);
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
                var result = await _accountService.RestoreAccount(id);
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