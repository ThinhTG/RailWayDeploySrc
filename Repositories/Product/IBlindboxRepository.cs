using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.Product
{
    public interface IBlindBoxRepository
    {
        Task<IEnumerable<BlindBox>> GetAllAsync();
        Task<BlindBox> GetByIdAsync(int id);
        Task<BlindBox> AddAsync(BlindBox blindBox);
        Task<BlindBox> UpdateAsync(BlindBox blindBox);
        Task DeleteAsync(int id);
    }
}