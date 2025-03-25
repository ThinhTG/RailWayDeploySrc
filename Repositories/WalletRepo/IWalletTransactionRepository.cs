using Models;

namespace Repositories.WalletRepo
{
    public interface IWalletTransactionRepository
    {
        Task AddWalletTransactionAsync(WalletTransaction walletTransaction);
        Task<IEnumerable<WalletTransaction>> GetWalletTransactionByWalletIdAsync(Guid walletId);
        Task<IEnumerable<WalletTransaction>> GetAllAsync();
        IQueryable<WalletTransaction> GetAll();
    }
}