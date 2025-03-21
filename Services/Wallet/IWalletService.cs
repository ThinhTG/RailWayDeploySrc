using Models;

namespace BlindBoxSS.API.Services
{
    public interface IWalletService
    {
        Task<Wallet> GetWalletByAccountId(string accountId);
        Task AddMoneyToWalletAsync(string accountId, decimal amount, int orderCode);
        Task<bool> UseWalletForPurchaseAsync(string accountId, decimal amount, int? orderId);
        Task<bool> UpdateUserWalletAsync(Wallet updatedWallet);
    }
}
