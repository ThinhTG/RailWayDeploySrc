using Microsoft.AspNetCore.Http.HttpResults;
using Models;
using Repositories.AddressRepo;
using Repositories.Pagging;
using Services.DTO;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.AddressS
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        public AddressService(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task<Address> AddAsync(AddAddressDTO addAddressDTO)
        {
            if (addAddressDTO == null)
            {
                throw new ArgumentNullException(nameof(addAddressDTO), "Voucher data is required.");
            }

          
            var AddressId = new Guid();
            var address = new Address
            {
                AccountId = addAddressDTO.AccountId,
                AddressLine1 = addAddressDTO.AddressLine1,
                AddressLine2 = addAddressDTO.AddressLine2,
                City = addAddressDTO.City,
                State = addAddressDTO.State,
                PostalCode =addAddressDTO.PostalCode,
                Country = addAddressDTO.Country,
               PhoneNumber = addAddressDTO.PhoneNumber,
                NameReceiver = addAddressDTO.NameReceiver,
                IsDefault = addAddressDTO.IsDefault,
                CreatedAt = addAddressDTO.CreatedAt.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(addAddressDTO.CreatedAt, DateTimeKind.Utc)
            : addAddressDTO.CreatedAt.ToUniversalTime(),
                UpdatedAt = addAddressDTO.UpdatedAt.Kind == DateTimeKind.Unspecified
            ? DateTime.SpecifyKind(addAddressDTO.UpdatedAt, DateTimeKind.Utc)
            : addAddressDTO.UpdatedAt.ToUniversalTime()

            };
            await _addressRepository.AddAsync(address);
            return address;

        }

        public async Task DeleteAsync(Guid id)
        {
            var address = await _addressRepository.GetByIdAsync(id);
            if (address == null)
            {
                throw new KeyNotFoundException($"Address with ID {id} not found.");
            }
            await _addressRepository.DeleteAsync(id);
          
        }

        public async Task<PaginatedList<Address>> GetAll(int pageNumber, int pageSize)
        {
            IQueryable<Address> address = _addressRepository.GetAll().AsQueryable();
            return await PaginatedList<Address>.CreateAsync(address, pageNumber, pageSize);
        }

        public async Task<IEnumerable<Address>> GetAllAsync()
        {
            return await _addressRepository.GetAddressAsync();
        }

        public async Task<PaginatedList<Address>> GetByAccountId(string accountId, int pageNumber, int pageSize)
        {
            IQueryable<Address> address = _addressRepository.GetByAccountId(accountId).AsQueryable();

            return await PaginatedList<Address>.CreateAsync(address, pageNumber, pageSize);
        }

        public async Task<Address> GetByIdAsync(Guid? id)
        {
            return await _addressRepository.GetByIdAsync(id);
        }

        public async Task<Address> GetDefaultAddressByAccoutId(string accountId)
        {
           var address = _addressRepository.GetByAccountId(accountId).FirstOrDefault(a => a.IsDefault);
            if (address == null)
            {
                throw new KeyNotFoundException($"Default address for account with ID {accountId} not found.");
            }
            return address;
        }

        public async Task<Address> UpdateAsync(Guid addressId, UpdateAddressDTO updateAddressDTO)
        {
            if (updateAddressDTO == null)
            {
                throw new ArgumentNullException(nameof(updateAddressDTO), "Address data is required.");
            }
            var address = await _addressRepository.GetByIdAsync(addressId);
            if (address == null)
            {
                throw new KeyNotFoundException($"Address with ID {addressId} not found.");
            }
            address.AddressLine1 = updateAddressDTO.AddressLine1;
            address.AddressLine2 = updateAddressDTO.AddressLine2; // Nullable field
            address.PhoneNumber = updateAddressDTO.PhoneNumber; 

            address.NameReceiver = updateAddressDTO.NameReceiver; 
            address.City = updateAddressDTO.City;
            address.State = updateAddressDTO.State; // Nullable field
            address.PostalCode = updateAddressDTO.PostalCode; // Nullable field
            address.Country = updateAddressDTO.Country;
            address.IsDefault = updateAddressDTO.IsDefault;
            address.UpdatedAt = updateAddressDTO.UpdatedAt.Kind == DateTimeKind.Unspecified
                ? DateTime.SpecifyKind(updateAddressDTO.UpdatedAt, DateTimeKind.Utc)
                : updateAddressDTO.UpdatedAt.ToUniversalTime();

            // Save the updated address to the repository
            await _addressRepository.UpdateAsync(address);
            return address;
        }
    }
}
