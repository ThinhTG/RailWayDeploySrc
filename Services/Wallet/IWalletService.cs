using Models;

namespace BlindBoxSS.API.Services
{
    public interface IWalletService
    {
        Task<Wallet> GetWalletByAccountId(Guid accountId);
        Task AddMoneyToWalletAsync(Guid accountId, decimal amount, int orderCode);
        Task<bool> UseWalletForPurchaseAsync(Guid accountId, decimal amount, int? orderId);
        Task<bool> UpdateUserWalletAsync(Wallet updatedWallet);
    }
}
