using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.AccountService;
using static DAO.Contracts.UserRequestAndResponse;

namespace BlindBoxSS.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
         private readonly IAccountService _accountService;

        public AdminController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        [HttpGet("GetAll")]
        [Authorize("AdminPolicy")]
        public async Task<IActionResult> GetAllAccounts(int pageNumber, int pageSize)
        {
            var accounts = await _accountService.GetAllAccountsAsync(pageNumber, pageSize);
            return Ok(accounts);
        }

        [HttpPut("{id}")]
        [Authorize("AdminPolicy")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request data.");
            }

            try
            {
                var updatedUser = await _accountService.AdminUpdateAsync(id, request);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }


    }
}
