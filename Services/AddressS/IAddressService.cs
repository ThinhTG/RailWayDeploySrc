using Models;
using Repositories.Pagging;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.AddressS
{
    public interface IAddressService
    {

        Task<IEnumerable<Address>> GetAllAsync();
        Task<Address> GetByIdAsync(Guid? id);
        Task<Address> AddAsync(AddAddressDTO addAddressDTO);
        Task<Address> UpdateAsync(Guid id,UpdateAddressDTO updateAddressDTO);
        Task DeleteAsync(Guid id);
        Task<PaginatedList<Address>> GetAll(int pageNumber, int pageSize);

        Task<PaginatedList<Address>> GetByAccountId(string accountId, int pageNumber, int pageSize);

        Task<Address> GetDefaultAddressByAccoutId(string accountId);
    }
}
