using DAO;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Product
{
    public class PackageRepository : IPackageRepository
    {
        private readonly BlindBoxDbContext _context;

        public PackageRepository(BlindBoxDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Package>> GetAllPackagesAsync()
        {
            return await _context.Set<Package>().ToListAsync();
        }

        public async Task<Package?> GetPackageByIdAsync(Guid id)
        {
            return await _context.Set<Package>().Include(p => p.BlindBoxes).FirstOrDefaultAsync(p => p.PackageId == id);
        }

        public async Task<List<Package>> GetPackageByTypeSell(string typeSell)
        {
            return await _context.Packages.Where(p => p.TypeSell == typeSell).ToListAsync();
        }

        public async Task<Package> AddPackageAsync(Package package)
        {
            _context.Set<Package>().Add(package);
            await _context.SaveChangesAsync();
            return package;
        }

        public async Task<Package?> UpdatePackageAsync(Package package)
        {
            var existingPackage = await _context.Set<Package>().FindAsync(package.PackageId);
            if (existingPackage == null)
            {
                return null;
            }

            _context.Entry(existingPackage).CurrentValues.SetValues(package);
            await _context.SaveChangesAsync();
            return existingPackage;
        }

        public async Task<bool> DeletePackageAsync(Guid id)
        {
            var package = await _context.Set<Package>().FindAsync(id);
            if (package == null)
            {
                return false;
            }

            _context.Set<Package>().Remove(package);
            await _context.SaveChangesAsync();
            return true;
        }

        public IQueryable<Package> GetAll()
        {
            return _context.Packages.AsQueryable();
        }
    }
}