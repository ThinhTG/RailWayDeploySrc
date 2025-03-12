using BlindBoxSS.API.Services;
using Microsoft.AspNetCore.Mvc;
using Services.Wallet;

[Route("api/wallets")]
[ApiController]
public class WalletController : ControllerBase
{
    private readonly IWalletService _walletService;
    private readonly IWalletTransactionService _walletTransactionService;
    private readonly ILogger<WalletController> _logger;

    public WalletController(
        IWalletService walletService,
        IWalletTransactionService walletTransactionService,
        ILogger<WalletController> logger)
    {
        _walletService = walletService;
        _walletTransactionService = walletTransactionService;
        _logger = logger;
    }

    // Lấy thông tin ví của người dùng
    [HttpGet("{accountId}")]
    public async Task<IActionResult> GetWallet(string accountId)
    {
        if (string.IsNullOrEmpty(accountId))
            return BadRequest(new { Message = "Invalid accountId." });

        try
        {
            var wallet = await _walletService.GetWalletByAccountId(accountId);
            if (wallet == null)
                return NotFound(new { Message = "Wallet not found." });

            return Ok(wallet);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching wallet for accountId: {AccountId}", accountId);
            return StatusCode(500, new { Message = "Internal Server Error. Please try again later." });
        }
    }

    // Nạp tiền vào ví
    [HttpPost("{accountId}/deposit")]
    public async Task<IActionResult> AddMoney(string accountId, [FromQuery] int amount)
    {
        if (string.IsNullOrEmpty(accountId) || amount <= 0)
            return BadRequest(new { Message = "Invalid accountId or amount." });

        try
        {
            await _walletService.AddMoneyToWalletAsync(accountId, amount);
            return Ok(new { Message = "Deposit successful. Wallet updated." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding money to wallet for accountId: {AccountId}", accountId);
            return StatusCode(500, new { Message = "Internal Server Error. Please try again later." });
        }
    }

    // Thanh toán từ ví
    [HttpPost("{accountId}/purchase")]
    public async Task<IActionResult> Purchase(string accountId, [FromQuery] int amount, [FromQuery] int? orderId)
    {
        if (string.IsNullOrEmpty(accountId) || amount <= 0)
            return BadRequest(new { Message = "Invalid accountId or amount." });

        try
        {
            var success = await _walletService.UseWalletForPurchaseAsync(accountId, amount, orderId);
            if (!success)
                return BadRequest(new { Message = "Insufficient balance or transaction failed." });

            return Ok(new { Message = "Purchase successful. Wallet updated." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing purchase for accountId: {AccountId}", accountId);
            return StatusCode(500, new { Message = "Internal Server Error. Please try again later." });
        }
    }
}
