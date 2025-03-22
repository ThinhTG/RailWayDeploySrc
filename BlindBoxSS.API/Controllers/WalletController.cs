﻿using BlindBoxSS.API.Services;
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

   /// <summary>
   /// Lấy tt Ví người dùng
   /// </summary>
   /// <param name="accountId"></param>
   /// <returns></returns>
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

    /// <summary>
    /// Nạp Tiền
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="amount"></param>
    /// <param name="orderCode"></param>
    /// <returns></returns>
    [HttpPost("{accountId}/deposit")]
    public async Task<IActionResult> AddMoney(string accountId, [FromQuery] decimal amount, int orderCode)
    {
        if (string.IsNullOrEmpty(accountId) || amount <= 0)
            return BadRequest(new { Message = "Invalid accountId or amount." });

        try
        {
            await _walletService.AddMoneyToWalletAsync(accountId, amount, orderCode);
            return Ok(new { Message = "Deposit successful. Wallet updated." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding money to wallet for accountId: {AccountId}", accountId);
            return StatusCode(500, new { Message = "Internal Server Error. Please try again later." });
        }
    }

    /// <summary>
    /// Thanh Toán Bằng ví
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="amount"></param>
    /// <param name="orderId"></param>
    /// <returns></returns>
    [HttpPost("{accountId}/purchase")]
    public async Task<IActionResult> Purchase(string accountId, [FromQuery] decimal amount, [FromQuery] int? orderId)
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
