using Models;
using Repositories.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Product
{
    public class PackageService : IPackageService
    {
        private readonly IPackageRepository _packageRepository;

        public PackageService(IPackageRepository packageRepository)
        {
            _packageRepository = packageRepository;
        }

        public async Task<IEnumerable<Package>> GetAllPackagesAsync()
        {
            return await _packageRepository.GetAllPackagesAsync();
        }

        public async Task<Package?> GetPackageByIdAsync(int id)
        {
            return await _packageRepository.GetPackageByIdAsync(id);
        }

        public async Task<Package> AddPackageAsync(Package package)
        {
            return await _packageRepository.AddPackageAsync(package);
        }

        public async Task<Package?> UpdatePackageAsync(Package package)
        {
            return await _packageRepository.UpdatePackageAsync(package);
        }

        public async Task<bool> DeletePackageAsync(int id)
        {
            return await _packageRepository.DeletePackageAsync(id);
        }
    }
}
