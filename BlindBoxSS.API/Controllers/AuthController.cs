using DAO.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.AccountService;
using Services.Email;
using Services.Request;
using static DAO.Contracts.UserRequestAndResponse;


namespace BlindBoxSS.API.Controllers
{
    [Route("api/")]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly SignInManager<ApplicationUser> _signInManager;

      
        public AuthController(IAccountService userService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailService emailService)
        {
            _accountService = userService;
            _userManager = userManager;
            _emailService = emailService;
            _signInManager = signInManager;

        }

       
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
        {
            var response = await _accountService.RegisterAsync(request);
            return Ok(response);
        }

    
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var response = await _accountService.LoginAsync(request);
            if (response == null)
            {
                return Unauthorized(new ErrorResponse
                {
                    Title = "Email Not Confirmed",
                    StatusCode = 401,
                    Message = "Email chưa được xác nhận! Vui lòng kiểm tra email của bạn."
                });
            }
            return Ok(response);
        }

       
        [HttpGet("user/{id}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _accountService.GetByIdAsync(id);
            return Ok(response);
        }

       
        [HttpPost("refresh-token")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var response = await _accountService.RefreshTokenAsync(request);
            return Ok(response);
        }

      
        [HttpPost("revoke-refresh-token")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> RevokeRefreshToken([FromBody] RefreshTokenRequest request)
        {
            var response = await _accountService.RevokeRefreshToken(request);
            if (response != null && response.Message == "Refresh token revoked successfully")
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

      
        [HttpDelete("user/{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _accountService.DeleteAsync(id);
            return Ok();
        }

     
        [HttpGet("confirm-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return BadRequest(new ErrorResponse
                {
                    Title = "Invalid Request",
                    StatusCode = 400,
                    Message = "Thông tin xác thực không hợp lệ."
                });
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new ErrorResponse
                {
                    Title = "User Not Found",
                    StatusCode = 404,
                    Message = "Không tìm thấy người dùng."
                });
            }


            string decodedToken = Uri.UnescapeDataString(token);
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
            if (result.Succeeded)
            {
                return Ok(new { Message = "Xác thực email thành công! Bạn có thể đăng nhập ngay bây giờ." });
            }

            return BadRequest(new ErrorResponse
            {
                Title = "Email Confirmation Failed",
                StatusCode = 400,
                Message = "Xác thực email thất bại. Token có thể đã hết hạn hoặc không hợp lệ."
            });
        }

        [HttpPost("resend-confirm-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ResendConfirmEmail([FromBody] ResendConfirmEmailRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return NotFound(new ErrorResponse
                {
                    Title = "User Not Found",
                    StatusCode = 404,
                    Message = "Không tìm thấy người dùng với email này."
                });
            }

            if (user.EmailConfirmed)
            {
                return BadRequest(new ErrorResponse
                {
                    Title = "Email Already Confirmed",
                    StatusCode = 400,
                    Message = "Email đã được xác nhận trước đó."
                });
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await _emailService.ResendConfirmationEmailAsync(user, token);

            return Ok(new { Message = "Email xác thực đã được gửi lại!" });
        }


        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] DAO.Contracts.UserRequestAndResponse.ForgotPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return NotFound(new ErrorResponse
                {
                    Title = "User Not Found",
                    StatusCode = 404,
                    Message = "Không tìm thấy người dùng với email này."
                });
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _emailService.SendResetPasswordEmailAsync(user, token);

            return Ok(new { Message = "Email đặt lại mật khẩu đã được gửi!" });
        }


        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody] DAO.Contracts.UserRequestAndResponse.ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return NotFound(new ErrorResponse
                {
                    Title = "User Not Found",
                    StatusCode = 404,
                    Message = "Không tìm thấy người dùng với email này."
                });
            }

            var decodedToken = Uri.UnescapeDataString(request.Token);
            var result = await _userManager.ResetPasswordAsync(user, decodedToken, request.NewPassword);

            if (result.Succeeded)
            {
                return Ok(new { Message = "Mật khẩu đã được đặt lại thành công!" });
            }

            return BadRequest(new ErrorResponse
            {
                Title = "Reset Password Failed",
                StatusCode = 400,
                Message = "Đặt lại mật khẩu thất bại. Token có thể đã hết hạn hoặc không hợp lệ."
            });
        }

        [HttpPost("google-login")]
        [AllowAnonymous]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)
        {
            var response = await _accountService.LoginGoogle(request);
            if (response == null)
            {
                return Unauthorized(new ErrorResponse
                {
                    Title = "Email Not Confirmed",
                    StatusCode = 401,
                    Message = "Email chưa được xác nhận! Vui lòng kiểm tra email của bạn."
                });
            }
            return Ok(response);

        }
    }
    }


