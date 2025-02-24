using Google;
using Microsoft.EntityFrameworkCore;
using Models;
using DAO;

namespace Repositories.WalletRepo
{
    public class WalletTransactionRepository : IWalletTransactionRepository
    {
        private readonly BlindBoxDbContext _context;

        public WalletTransactionRepository(BlindBoxDbContext context)
        {
            _context = context;
        }

        public async Task AddWalletTransactionAsync(WalletTransaction walletTransaction)
        {
            await _context.WalletTransaction.AddAsync(walletTransaction);
            await _context.SaveChangesAsync();
        }
    }
}
