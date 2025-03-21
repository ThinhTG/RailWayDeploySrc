using Models;
using Repositories.Pagging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Product
{
    public interface IPackageService
    {
        Task<IEnumerable<Package>> GetAllPackagesAsync();
        Task<Package?> GetPackageByIdAsync(Guid id);
        Task<Package> AddPackageAsync(Package package);
        Task<Package?> UpdatePackageAsync(Package package);
        Task<bool> DeletePackageAsync(Guid id);
        Task<IEnumerable<Package>> GetAllAsync(string? searchByCategory, string? searchByName, decimal? minPrice, decimal? maxPrice);
        Task<PaginatedList<Package>> GetAllFilter(string? searchByCategory, string? searchByName, decimal? minPrice, decimal? maxPrice, int pageNumber, int pageSize);
        Task<List<Package>> GetPackageByTypeSell(string typeSell);
    }
}
