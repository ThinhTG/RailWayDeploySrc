using DAO;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.VocherRepo
{
    public class VocherRepository : IVocherRepository
    {
        private readonly BlindBoxDbContext _context;

        public VocherRepository(BlindBoxDbContext context)
        {
            _context = context;
        }

        public async Task<Voucher> AddAsync(Voucher voucher)
        {
            _context.Set<Voucher>().Add(voucher);
            await _context.SaveChangesAsync();
            return voucher;
        }

        public async Task DeleteAsync(Guid id)
        {
            var voucher = await _context.Set<Voucher>().FindAsync(id);
            if (voucher != null)
            {
                _context.Set<Voucher>().Remove(voucher);
                await _context.SaveChangesAsync();
            }
        }

        public IQueryable<Voucher> GetAll()
        {
            return _context.Vouchers.AsQueryable();
        }

        public async Task<Voucher> GetByIdAsync(Guid id)
        {
            return await _context.Set<Voucher>().FindAsync(id);
        }

        public async Task<IEnumerable<Voucher>> GetVouchersAsync()
        {
            return await _context.Set<Voucher>().ToListAsync();
        }

        public async Task<Voucher> UpdateAsync(Voucher voucher)
        {
            _context.Entry(voucher).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return voucher;
        }
    }
}
