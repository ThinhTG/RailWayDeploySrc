using Models;
using Repositories.WalletRepo;
using Services.Wallet;

namespace BlindBoxSS.API.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IWalletTransactionService _walletTransactionService;

        public WalletService(IWalletRepository walletRepository, IWalletTransactionService walletTransactionService)
        {
            _walletRepository = walletRepository;
            _walletTransactionService = walletTransactionService;
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

        public async Task AddMoneyToWalletAsync(string accountId, int amount)
        {
            var wallet = await _walletRepository.GetWalletByAccountIdAsync(accountId);
            if (wallet == null)
            {
                throw new Exception("Wallet not found");
            }

            wallet.Balance += amount;
            await _walletRepository.UpdateWalletAsync(wallet);
            try
            {
                await _walletTransactionService.AddWalletTransactionAsync(wallet.WalletId, amount, "deposit", "success", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"), wallet.Balance, null);
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UseWalletForPurchaseAsync(string accountId, int amount, int? orderId)
        {
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

            var walletTransaction = new WalletTransaction
            {
                WalletId = wallet.WalletId,
                Amount = amount,
                TransactionType = "Debit",
                TransactionStatus = "Success",
                TransactionDate = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                TransactionBalance = wallet.Balance.ToString(),
                OrderId = orderId
            };

            await _walletTransactionService.AddWalletTransactionAsync(wallet.WalletId, amount, "purchase", "success", DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"), wallet.Balance, orderId);
            return true;
        }
    }
}