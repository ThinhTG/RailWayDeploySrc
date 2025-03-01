using DAO.Contracts;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Mvc;
using Models;
using Services.AccountService;
using Services.Email;

using static DAO.Contracts.UserRequestAndResponse;

using Services.Request;
using Services.DTO;


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

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthController"/> class.
        /// </summary>
        /// <param name="userService">The user service for managing user-related operations.</param>
        public AuthController(IAccountService userService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailService emailService)
        {
            _accountService = userService;
            _userManager = userManager;
            _emailService = emailService;
            _signInManager = signInManager;

        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="request">The user registration request.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
        {
            var response = await _accountService.RegisterAsync(request);
            return Ok(response);
        }

        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="request">The user login request.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
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

        /// <summary>
        /// Gets a user by ID.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        [HttpGet("user/{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _accountService.GetByIdAsync(id);
            return Ok(response);
        }

        /// <summary>
        /// Refreshes the access token using the refresh token.
        /// </summary>
        /// <param name="request">The refresh token request.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        [HttpPost("refresh-token")]
        [Authorize]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var response = await _accountService.RefreshTokenAsync(request);
            return Ok(response);
        }

        /// <summary>
        /// Revokes the refresh token.
        /// </summary>
        /// <param name="request">The refresh token request to be revoked.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
        [HttpPost("revoke-refresh-token")]
        [Authorize]
        public async Task<IActionResult> RevokeRefreshToken([FromBody] RefreshTokenRequest request)
        {
            var response = await _accountService.RevokeRefreshToken(request);
            if (response != null && response.Message == "Refresh token revoked successfully")
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        /// <summary>
        /// Deletes a user.
        /// </summary>
        /// <param name="id">The ID of the user to be deleted.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the operation.</returns>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageNumber">trang so bn</param>
        /// <param name="pageSize">so phan tu</param>
        /// <returns></returns>
        [HttpGet("GetAll")]
        //[Authorize("AdminPolicy")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAccounts( int pageNumber, int pageSize)
        {
            var accounts = await _accountService.GetAllAccountsAsync( pageNumber, pageSize);
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


