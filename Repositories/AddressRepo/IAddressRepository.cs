using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.AddressRepo
{
   public interface IAddressRepository
    {

        Task<IEnumerable<Address>> GetAddressAsync();
        Task<Address> GetByIdAsync(Guid id);
        Task<Address> AddAsync(Address address);
        Task<Address> UpdateAsync(Address address);
        Task DeleteAsync(Guid id);
        IQueryable<Address> GetAll();
        IQueryable<Address> GetByAccountId(string id);

    }
}
