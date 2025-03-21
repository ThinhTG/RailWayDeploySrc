using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.AccountService;
using Services.DTO;

namespace BlindBoxSS.API.Controllers
{
    [Route("api/Account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _accountService.GetByIdAsync(id));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _accountService.DeleteAsync(id);
            return Ok();
        }

        [HttpPut("update-avatar")]
        public async Task<IActionResult> UpdateAvatar([FromBody] UpdateAvatarDTO model)
        {
            var isUpdated = await _accountService.UpdateAvatarAsync(model.AccountId, model.AvatarURL);
            if (isUpdated == null) return NotFound("User not found");

            return Ok(new { message = "Avatar updated successfully" });
        }

        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDTO model)
        {
            var isUpdated = await _accountService.UpdateAccount(model);
            if (isUpdated == null)
                throw new KeyNotFoundException("Update profile failed");

            return Ok(new { message = "Profile updated successfully" });
        }

    }
}
