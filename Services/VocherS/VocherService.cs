using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.CategoryRepo;
using Repositories.Pagging;
using Repositories.VocherRepo;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.VocherS
{
    public  class VocherService :IVocherService
    {
        private readonly IVocherRepository _vocherRepository;

        public VocherService(IVocherRepository vocherRepository)
        {
            _vocherRepository = vocherRepository;
        }

        public async Task<Voucher> AddVoucherAsync(AddVoucherDTO createVocherDto)
        {
            if (createVocherDto == null)
            {
                throw new ArgumentNullException(nameof(createVocherDto), "Voucher data is required.");
            }

            if (createVocherDto.EndDate <= createVocherDto.StartDate)
            {
                throw new ArgumentException("EndDate must be after StartDate.", nameof(createVocherDto.EndDate));
            }

            var voucherId = new Guid();
            var existingVoucher = await _vocherRepository.GetByIdAsync(voucherId);
            if (existingVoucher != null)
            {
                throw new InvalidOperationException($"A voucher with ID {voucherId} already exists.");
            }

            var voucher = new Voucher
            {
                VoucherId = voucherId,
                Quantity = createVocherDto.Quantity,
                VoucherCode = createVocherDto.VoucherCode,
                OrderId = null, // Set to null as per requirement
                Description = createVocherDto.Description,
                DiscountMoney = createVocherDto.DiscountMoney,
                Money = createVocherDto.Money,
                StartDate = createVocherDto.StartDate.Kind == DateTimeKind.Unspecified
                    ? DateTime.SpecifyKind(createVocherDto.StartDate, DateTimeKind.Utc)
                    : createVocherDto.StartDate.ToUniversalTime(),
                EndDate = createVocherDto.EndDate.Kind == DateTimeKind.Unspecified
                    ? DateTime.SpecifyKind(createVocherDto.EndDate, DateTimeKind.Utc)
                    : createVocherDto.EndDate.ToUniversalTime()
            };

            await _vocherRepository.AddAsync(voucher);
            return voucher;
        }

        public async Task DeleteVoucherAsync(Guid id)
        {
            var voucher = await _vocherRepository.GetByIdAsync(id);
            if (voucher == null)
            {
                throw new KeyNotFoundException($"Voucher with ID {id} not found.");
            }

            // Prevent deletion if the voucher is associated with an order
            if (voucher.OrderId.HasValue)
            {
                throw new InvalidOperationException("Cannot delete a voucher that is already associated with an order.");
            }
            await _vocherRepository.DeleteAsync(id);
        }

        public async Task<PaginatedList<Voucher>> GetAll(int pageNumber, int pageSize)
        {
            IQueryable<Voucher> vocher = _vocherRepository.GetAll().AsQueryable();
            return await PaginatedList<Voucher>.CreateAsync(vocher, pageNumber, pageSize);
        }

        public async Task<IEnumerable<Voucher>> GetAllVouchersAsync()
        {
            return await _vocherRepository.GetVouchersAsync();
        }

        public async Task<Voucher?> GetVoucherByIdAsync(Guid id)
        {
            return await _vocherRepository.GetByIdAsync(id);
        }

        public async Task<Voucher?> UpdateVoucherAsync(Guid id, UpdateVoucherDTO updateVoucherDto)
        {
            if (updateVoucherDto == null)
            {
                throw new ArgumentNullException(nameof(updateVoucherDto), "Voucher update data is required.");
            }

            if (updateVoucherDto.EndDate <= updateVoucherDto.StartDate)
            {
                throw new ArgumentException("EndDate must be after StartDate.", nameof(updateVoucherDto.EndDate));
            }

            var voucher = await _vocherRepository.GetByIdAsync(id);
            if (voucher == null)
            {
                return null; // Return null to indicate not found
            }

            // Prevent update if the voucher is associated with an order
            if (voucher.OrderId.HasValue)
            {
                throw new InvalidOperationException("Cannot update a voucher that is already associated with an order.");
            }

            // Update only the allowed fields
            voucher.Description = updateVoucherDto.Description;
            voucher.DiscountMoney = updateVoucherDto.DiscountMoney;
            voucher.Money = updateVoucherDto.Money;
            voucher.StartDate = updateVoucherDto.StartDate.Kind == DateTimeKind.Unspecified
                ? DateTime.SpecifyKind(updateVoucherDto.StartDate, DateTimeKind.Utc)
                : updateVoucherDto.StartDate.ToUniversalTime();
            voucher.EndDate = updateVoucherDto.EndDate.Kind == DateTimeKind.Unspecified
                ? DateTime.SpecifyKind(updateVoucherDto.EndDate, DateTimeKind.Utc)
                : updateVoucherDto.EndDate.ToUniversalTime();

            await _vocherRepository.UpdateAsync(voucher);
            return voucher;
        }

        public async Task<Results<Ok<List<VoucherResponse>>, NotFound>> GetAvailableVouchersAsync(decimal TotalPrice)
        {
            if (TotalPrice < 0)
            {
                return TypedResults.NotFound();
            }

            var vouchersQuery = _vocherRepository.GetAll();

            if (!vouchersQuery.Any())
            {
                return TypedResults.Ok(new List<VoucherResponse>());
            }


            var availableVouchers = await vouchersQuery
                .Where(v => v.StartDate <= DateTime.UtcNow && v.EndDate > DateTime.UtcNow && TotalPrice >= v.Money)
                .Select(v => new VoucherResponse
                {
                    VoucherId = v.VoucherId,
                    Description = v.Description,
                    DiscountMoney = v.DiscountMoney,
                    TotalPrice = v.Money,
                    EndDate = v.EndDate
                })
                .ToListAsync();

            return availableVouchers.Any() ? TypedResults.Ok(availableVouchers) : TypedResults.NotFound();
        }




    }
}
