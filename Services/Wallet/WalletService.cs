using Microsoft.Extensions.Configuration;
using Models;
using Repositories.WalletRepo;
using Services.Payment;
using Services.Wallet;
using System.Globalization;
using TimeZoneConverter;

namespace BlindBoxSS.API.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IWalletTransactionService _walletTransactionService;
        private readonly IPaymentService _paymentService;
        private readonly IConfiguration _configuration;

        public WalletService(IWalletRepository walletRepository, IWalletTransactionService walletTransactionService,IConfiguration configuration, IPaymentService paymentService)
        {
            _walletRepository = walletRepository;
            _walletTransactionService = walletTransactionService;
            _paymentService = paymentService;
            _configuration = configuration;
        }

        public async Task<Wallet> CreateWallet(Wallet wallet)
        {
            try
            {
                var newWallet = await _walletRepository.CreateWallet(wallet);
                return newWallet;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Wallet> GetWalletByAccountId(string accountId)
        {
            var wallet = await _walletRepository.GetWalletByAccountIdAsync(accountId);
            if (wallet == null)
            {
                throw new Exception("Wallet not found");
            }

            return wallet;
        }

        public async Task AddMoneyToWalletAsync(string accountId, decimal amount,int orderCode)
        {
            var dateFormat = _configuration["TransactionSettings:DateFormat"] ?? "yyyy-MM-ddTHH:mm:ssZ";
            bool useUTC = bool.TryParse(_configuration["TransactionSettings:UseUTC"], out bool utc) && utc;
            var timeZoneId = _configuration["TransactionSettings:TimeZone"] ?? "UTC";
            DateTime transactionDatetime = DateTime.UtcNow; // Default to UTC

            var wallet = await _walletRepository.GetWalletByAccountIdAsync(accountId);
            if (wallet == null)
            {
                throw new Exception("Wallet not found");
            }

            //check payment status
            var checkingPayment = await _paymentService.GetPaymentLinkInformationAsync(orderCode);
                if (checkingPayment.status == "PAID")
            {
                wallet.Balance += amount;
                await _walletRepository.UpdateWalletAsync(wallet);
            }
           

            if (!useUTC)
            {
                try
                {
                    // Convert UTC time to specified TimeZone
                    TimeZoneInfo timeZone = TZConvert.GetTimeZoneInfo(timeZoneId);
                    transactionDatetime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
                }
                catch (TimeZoneNotFoundException)
                {
                    throw new Exception("Invalid TimeZone");
                }
            }

            try
            {
                
                await _walletTransactionService.AddWalletTransactionAsync(wallet.WalletId, amount, "deposit", "success", transactionDatetime.ToString(dateFormat, CultureInfo.InvariantCulture), wallet.Balance, null);
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UseWalletForPurchaseAsync(string accountId, decimal amount, int? orderId)
        {
            var dateFormat = _configuration["TransactionSettings:DateFormat"] ?? "yyyy-MM-ddTHH:mm:ssZ";
            bool useUTC = bool.TryParse(_configuration["TransactionSettings:UseUTC"], out bool utc) && utc;
            var timeZoneId = _configuration["TransactionSettings:TimeZone"] ?? "UTC";
            DateTime transactionDatetime = DateTime.UtcNow; // Default to UTC

            var wallet = await _walletRepository.GetWalletByAccountIdAsync(accountId);
            if (wallet == null)
            {
                throw new Exception("Wallet not found");
            }
            if (wallet.Balance < amount)
            {
                return false;
            }
            wallet.Balance -= amount;
            await _walletRepository.UpdateWalletAsync(wallet);

            if (!useUTC)
            {
                try
                {
                    // Convert UTC time to specified TimeZone
                    TimeZoneInfo timeZone = TZConvert.GetTimeZoneInfo(timeZoneId);
                    transactionDatetime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
                }
                catch (TimeZoneNotFoundException)
                {
                    throw new Exception("Invalid TimeZone");
                }
            }

            var walletTransaction = new WalletTransaction
            {
                WalletId = wallet.WalletId,
                Amount = amount,
                TransactionType = "Debit",
                TransactionStatus = "Success",
                TransactionDate = transactionDatetime.ToString(dateFormat, CultureInfo.InvariantCulture),
                TransactionBalance = wallet.Balance.ToString(),
                OrderId = orderId
            };

            await _walletTransactionService.AddWalletTransactionAsync(wallet.WalletId, amount, "purchase", "success", transactionDatetime.ToString(dateFormat, CultureInfo.InvariantCulture), wallet.Balance, orderId);
            return true;
        }
    }
}