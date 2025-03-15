using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Product
{
    public interface IPackageRepository
    {
        Task<IEnumerable<Package>> GetAllPackagesAsync();
        Task<Package?> GetPackageByIdAsync(Guid id);
        Task<Package> AddPackageAsync(Package package);
        Task<Package?> UpdatePackageAsync(Package package);
        Task<bool> DeletePackageAsync(Guid id);

        IQueryable<Package> GetAll();

    }
}
