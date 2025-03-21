using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.AccountService;
using static DAO.Contracts.UserRequestAndResponse;

[Route("api/admin")]
[ApiController]
[Authorize("AdminPolicy")]
public class AdminController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AdminController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    /// <summary>
    /// Lấy danh sách tài khoản có phân trang
    /// </summary>
    [HttpGet("accounts")]
    public async Task<IActionResult> GetAllAccounts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        if (pageNumber <= 0 || pageSize <= 0)
            return BadRequest(new { message = "Page number and size must be greater than zero." });

        var accounts = await _accountService.GetAllAccountsAsync(pageNumber, pageSize);
        return Ok(accounts);
    }

    /// <summary>
    /// Cập nhật thông tin người dùng
    /// </summary>
    [HttpPut("users/{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] AdminUpdateRequest request)
    {
        if (request == null)
            return BadRequest(new { message = "Invalid request data." });

        try
        {
            var updatedUser = await _accountService.AdminUpdateAsync(id, request);
            if (updatedUser == null)
                return NotFound(new { message = "User not found or update failed." });

            return Ok(updatedUser);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal Server Error.", error = ex.Message });
        }
    }
}
