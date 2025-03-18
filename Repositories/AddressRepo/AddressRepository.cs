using DAO;
using Microsoft.EntityFrameworkCore;
using Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.AddressRepo
{
    public class AddressRepository : IAddressRepository
    {
        private readonly BlindBoxDbContext _context;
        public AddressRepository(BlindBoxDbContext context)
        {
            _context= context;
        }

        public async Task<Address> AddAsync(Address address)
        {
            _context.Set<Address>().Add(address);
            await _context.SaveChangesAsync();
            return address;
        }

     

        public async Task<IEnumerable<Address>> GetAddressAsync()
        {
            return await _context.Set<Address>().ToListAsync();
        }

        public  IQueryable<Address> GetAll()
        {
            return _context.Address.AsQueryable();
        }

       

       

        public async Task<Address> UpdateAsync(Address address)
        {
            _context.Entry(address).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return address;
        }

        public IQueryable<Models.Address>GetByAccountId(string id)
        {
            return _context.Address.Where(o => o.AccountId == id);
        }

        public async Task<Address> GetByIdAsync(Guid id)
        {
            return await _context.Set<Address>().FindAsync(id);
        }

        public async Task DeleteAsync(Guid id)
        {
            var address = await _context.Set<Address>().FindAsync(id);
            if (address != null)
            {
                _context.Set<Address>().Remove(address);
                await _context.SaveChangesAsync();
            }
        }
    }
}
