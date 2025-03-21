using Models;
using Repositories.Pagging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Product
{
    public interface IBlindBoxService
    {
        Task<IEnumerable<BlindBox>> GetAllAsync(string? searchByCategory, string? searchByName, decimal? minPrice, decimal? maxPrice);
        Task<BlindBox> GetByIdAsync(Guid id);
        Task<BlindBox> AddAsync(BlindBox blindBox);
        Task<BlindBox> UpdateAsync(BlindBox blindBox);
        Task DeleteAsync(Guid id);

        Task<PaginatedList<BlindBox>> GetAll(int pageNumber, int pageSize);

        Task<PaginatedList<BlindBox>> GetAllFilter(string? searchByCategory, string? searchByName, decimal? minPrice, decimal? maxPrice, int pageNumber, int pageSize);

        Task<List<BlindBox>> GetBlindboxeByTypeSell(string typeSell);
    }
}
