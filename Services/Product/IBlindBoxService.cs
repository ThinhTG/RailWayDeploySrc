using DAO.Contracts;
using Models;
using Repositories.Pagging;

namespace Services.Product
{
    public interface IBlindBoxService
    {
        Task<IEnumerable<BlindBoxMobileResponse>> GetAllAsync(string? searchByCategory, string? searchByName, decimal? minPrice, decimal? maxPrice, string? size);
        Task<BlindBox> GetByIdAsync(Guid id);
        Task<BlindBox> AddAsync(BlindBox blindBox);
        Task<BlindBox> UpdateAsync(BlindBox blindBox);
        Task DeleteAsync(Guid id);

        Task<PaginatedList<BlindBox>> GetAll(int pageNumber, int pageSize);

        Task<PaginatedList<BlindBox>> GetAllFilter(string? searchByCategory, string? size , string? searchByName, decimal? minPrice, decimal? maxPrice, int pageNumber, int pageSize);
    }
}
