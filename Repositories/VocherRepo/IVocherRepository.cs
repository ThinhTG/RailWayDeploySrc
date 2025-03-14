using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.VocherRepo
{
    public interface IVocherRepository
    {
        Task<IEnumerable<Voucher>> GetVouchersAsync();
        Task<Voucher> GetByIdAsync(Guid id);
        Task<Voucher> AddAsync(Voucher voucher);
        Task<Voucher> UpdateAsync(Voucher voucher);
        Task DeleteAsync(Guid id);
        IQueryable<Voucher> GetAll();
    }
}
