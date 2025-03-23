using Google;
using Microsoft.EntityFrameworkCore;
using Models;
using DAO;

namespace Repositories.WalletRepo
{
    public class WalletRepository : IWalletRepository
    {
        private readonly BlindBoxDbContext _context;

        public WalletRepository(BlindBoxDbContext context)
        {
            _context = context;
        }

        public async Task<Wallet> CreateWallet(Wallet wallet)
        {
            await _context.Wallet.AddAsync(wallet);
            await _context.SaveChangesAsync();
            return wallet;
        }

        public async Task<Wallet> GetWalletByAccountIdAsync(Guid accountId)
        {
            if (accountId == null)
            {
                throw new ArgumentNullException(nameof(accountId));
            }
            return await _context.Wallet.FirstOrDefaultAsync(w => w.AccountId == accountId);
        }

        public async Task UpdateWalletAsync(Wallet wallet)
        {
            _context.Wallet.Update(wallet);
            await _context.SaveChangesAsync();
        }

        public async Task<Wallet> GetBalanceAccountIdAsync(Guid accountId)
        {
            if (accountId == null)
            {
                throw new ArgumentNullException(nameof(accountId));
            }
            return await _context.Wallet.FirstOrDefaultAsync(w => w.AccountId == accountId);
        }
    }
}
