using Models;

namespace BlindBoxSS.API.Services
{
    public interface IWalletService
    {
        Task<Wallet> GetWalletByAccountId(string accountId);
        Task AddMoneyToWalletAsync(string accountId, int amount, int orderCode);
        Task<bool> UseWalletForPurchaseAsync(string accountId, int amount, int? orderId);
    }
}
