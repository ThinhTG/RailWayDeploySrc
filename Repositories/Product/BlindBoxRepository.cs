using DAO;
using DAO.Contracts;
using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Pagging;
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
        public IQueryable<BlindBox> GetAll()
        {
            return _context.BlindBoxes.AsQueryable();
        }

        public async Task<List<BlindBox>> GetBlindBoxByTypeSell(string typeSell)
        {
            //var blindbox = await _context.BlindBoxes.Where(b => b.TypeSell == typeSell).FirstOrDefaultAsync();
            return await _context.BlindBoxes
                .Where(b => b.TypeSell == typeSell && b.Package.TypeSell == typeSell)
                .Include(b => b.Package)
                .ToListAsync();
        }

        public async Task<BlindBox> GetByIdAsync(Guid id)
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

        public async Task DeleteAsync(Guid id)
        {
            var blindBox = await _context.Set<BlindBox>().FindAsync(id);
            if (blindBox != null)
            {
                _context.Set<BlindBox>().Remove(blindBox);
                await _context.SaveChangesAsync();
            }
        }
        //public async Task<List<BlindBox>> GetBlindBoxesWithPackageByTypeSell(string typeSell)
        //{
        //    return await _context.BlindBoxes
        //        .Where(b => b.TypeSell == typeSell && b.Package.TypeSell == typeSell)
        //        .ToListAsync();
        //}

     

     public  async Task<IEnumerable<BlindBox>> GetAllMobileAsync()
        {
            return await _context.Set<BlindBox>().ToListAsync();
        }

        public async Task<IEnumerable<BlindBox>> GetBlindBoxByTypeSellPaged(string typeSell)
        {
            IQueryable<BlindBox> blindBoxes = _context.BlindBoxes.AsQueryable()
                .Where(b => b.TypeSell == typeSell && b.Package.TypeSell == typeSell)
                .Include(b => b.Package);
            return await blindBoxes.ToListAsync();
        }
    }
}