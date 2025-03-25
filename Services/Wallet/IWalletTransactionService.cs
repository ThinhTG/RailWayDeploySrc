using Models;
using Repositories.Pagging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Wallet
{
    public interface IWalletTransactionService
    {
        Task AddWalletTransactionAsync(Guid walletId, decimal amount, string transactionType, string transactionStatus, string transactionDate, decimal transacionBalance, int? orderId);
        Task<PaginatedList<WalletTransaction>> GetWalletTransactionByWalletIdAsync(Guid walletId, int pageNumber, int pageSize);
        Task<PaginatedList<WalletTransaction>> GetAll(int pageNumber, int pageSize);
    }
}
