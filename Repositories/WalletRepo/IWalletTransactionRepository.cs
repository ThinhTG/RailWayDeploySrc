using Models;

namespace Repositories.WalletRepo
{
    public interface IWalletTransactionRepository
    {
        Task AddWalletTransactionAsync(WalletTransaction walletTransaction);
    }
}