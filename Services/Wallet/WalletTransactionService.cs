using Models;
using Repositories.WalletRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Wallet
{
    public class WalletTransactionService : IWalletTransactionService
    {
        private readonly IWalletTransactionRepository _walletTransactionRepository;

        public WalletTransactionService(IWalletTransactionRepository walletTransactionRepository)
        {
            _walletTransactionRepository = walletTransactionRepository;
        }

        public async Task AddWalletTransactionAsync(Guid walletId, int amount, string transactionType, string transactionStatus, string transactionDate, int transacionBalance, int? orderId)
        {
            WalletTransaction walletTransaction = new WalletTransaction
            {
                WalletId = walletId,
                Amount = amount,
                TransactionType = transactionType,
                TransactionStatus = transactionStatus,
                TransactionDate = transactionDate,
                TransactionBalance = transacionBalance.ToString(),
                OrderId = orderId
            };
            await _walletTransactionRepository.AddWalletTransactionAsync(walletTransaction);
        }

        
    }
}
