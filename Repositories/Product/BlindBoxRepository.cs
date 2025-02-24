using DAO;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.Product
{
    public class BlindBoxRepository : IBlindBoxRepository
    {
        private readonly BlindBoxDbContext _context;

        public BlindBoxRepository(BlindBoxDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BlindBox>> GetAllAsync()
        {
            return await _context.Set<BlindBox>().ToListAsync();
        }

        public async Task<BlindBox> GetByIdAsync(int id)
        {
            return await _context.Set<BlindBox>().FindAsync(id);
        }

        public async Task<BlindBox> AddAsync(BlindBox blindBox)
        {
            _context.Set<BlindBox>().Add(blindBox);
            await _context.SaveChangesAsync();
            return blindBox;
        }

        public async Task<BlindBox> UpdateAsync(BlindBox blindBox)
        {
            _context.Entry(blindBox).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return blindBox;
        }

        public async Task DeleteAsync(int id)
        {
            var blindBox = await _context.Set<BlindBox>().FindAsync(id);
            if (blindBox != null)
            {
                _context.Set<BlindBox>().Remove(blindBox);
                await _context.SaveChangesAsync();
            }
        }
    }
}