using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Product
{
    public interface IBlindBoxService
    {
        Task<IEnumerable<BlindBox>> GetAllAsync();
        Task<BlindBox> GetByIdAsync(int id);
        Task<BlindBox> AddAsync(BlindBox blindBox);
        Task<BlindBox> UpdateAsync(BlindBox blindBox);
        Task DeleteAsync(int id);
    }
}
