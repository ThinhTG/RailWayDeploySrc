using Models;
using Repositories.Pagging;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.VocherS
{
    public interface IVocherService
    {
        Task<IEnumerable<Voucher>> GetAllVouchersAsync();
        Task<Voucher?> GetVoucherByIdAsync(Guid id);
        Task<Voucher> AddVoucherAsync(AddVoucherDTO createVoucherDto);
        Task<Voucher?> UpdateVoucherAsync(Guid id, UpdateVoucherDTO updateVoucherDto);
        Task DeleteVoucherAsync(Guid id);
        Task<PaginatedList<Voucher>> GetAll(int pageNumber, int pageSize);
    }
}
