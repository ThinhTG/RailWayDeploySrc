﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services.AccountService;
using Services.DTO;
using Services.Request;
using static DAO.Contracts.UserRequestAndResponse;

namespace BlindBoxSS.API.Controllers
{
    [Route("api/Auth")]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AuthController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
        {
            return Ok(await _accountService.RegisterAsync(request));
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            return Ok(await _accountService.LoginAsync(request));
        }

        [HttpGet("user/{id}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetById(string id)
        {
            return Ok(await _accountService.GetByIdAsync(id));
        }

        [HttpPost("refresh-token")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            return Ok(await _accountService.RefreshTokenAsync(request));
        }

        [HttpPost("revoke-refresh-token")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> RevokeRefreshToken([FromBody] RefreshTokenRequest request)
        {
            return Ok(await _accountService.RevokeRefreshToken(request));
        }

        [HttpDelete("user/{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _accountService.DeleteAsync(id);
            return Ok();
        }

        [HttpGet("confirm-email")]
        [HttpPost("confirm-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string token)
        {
            bool isConfirmed = await _accountService.ConfirmEmailAsync(userId, token);
            if (isConfirmed)
                return Redirect("http://localhost:3000/welcome");   // deploy sửa lại
            return BadRequest("Xác nhận email thất bại.");
        }


        [HttpPost("resend-confirm-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ResendConfirmEmail([FromBody] ResendConfirmEmailRequest request)
        {
            return Ok(await _accountService.ResendConfirmEmailAsync(request.Email));
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            await _accountService.ForgotPasswordAsync(request.Email);
            return Ok("Send Email Successfull, please check your email to reset Password");
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            return Ok(await _accountService.ResetPasswordAsync(request.Email, request.Token, request.NewPassword));
        }

        [HttpPost("google-login")]
        [AllowAnonymous]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)
        {
            return Ok(await _accountService.LoginGoogle(request));
        }


        /// <summary>
        /// Update Ảnh Đại diện
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("update-avatar")]
        public async Task<IActionResult> UpdateAvatar([FromBody] UpdateAvatarDTO model)
        {
            var isUpdated = await _accountService.UpdateAvatarAsync(model.AccountId,model.AvatarURL);
            if (isUpdated == null) return NotFound("User not found");

            return Ok(new { message = "Avatar updated successfully" });
        }


    }
}
