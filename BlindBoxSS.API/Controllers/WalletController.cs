using Microsoft.AspNetCore.Mvc;
using BlindBoxSS.API.Services;
using Microsoft.Extensions.Logging;
using Services.Wallet;
using Models;

namespace BlindBoxSS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;
        private readonly IWalletTransactionService _walletTransactionService;
        private readonly ILogger<WalletController> _logger;

        public WalletController(IWalletService walletService, IWalletTransactionService walletTransactionService,ILogger<WalletController> logger)
        {
            _walletService = walletService;
            _walletTransactionService = walletTransactionService;
            _logger = logger;
        }

        [HttpGet("getWallet")]
        public async Task<IActionResult> GetWallet([FromQuery] string accountId)
        {
            if (string.IsNullOrEmpty(accountId))
            {
                return BadRequest(new { Message = "Invalid accountId." });
            }
            try
            {
                var wallet = await _walletService.GetWalletByAccountId(accountId);
                return Ok(wallet);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching wallet for accountId: {AccountId}", accountId);
                return BadRequest(new { Message = "An error occurred while processing your request. Please try again later." });
            }
        }

        [HttpPost("addWallet")]
        public async Task<IActionResult> PaymentSuccess([FromQuery] string accountId, [FromQuery] int amount)
        {
            if (string.IsNullOrEmpty(accountId) || amount <= 0)
            {
                return BadRequest(new { Message = "Invalid accountId or amount." });
            }

            try
            {
                await _walletService.AddMoneyToWalletAsync(accountId, amount);
                return Ok(new { Message = "Payment successful and wallet updated." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding money to wallet for accountId: {AccountId}", accountId);
                return BadRequest(new { Message = "An error occurred while processing your request. Please try again later." });
            }
        }

        [HttpPost("purchase")]
        public async Task<IActionResult> Purchase([FromQuery] string accountId, [FromQuery] int amount, [FromQuery] int? orderId)
        {
            if (string.IsNullOrEmpty(accountId) || amount <= 0)
            {
                return BadRequest(new { Message = "Invalid accountId or amount." });
            }

            try
            {
                var result = await _walletService.UseWalletForPurchaseAsync(accountId, amount, orderId);
                if (!result)
                {
                    return BadRequest(new { Message = "Payment failed." });
                }
                return Ok(new { Message = "Payment successful and wallet updated." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing purchase for accountId: {AccountId}", accountId);
                return BadRequest(new { Message = "An error occurred while processing your request. Please try again later." });
            }
        }
    }
}