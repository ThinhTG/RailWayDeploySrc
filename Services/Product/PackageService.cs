using Microsoft.EntityFrameworkCore;
using Models;
using Repositories.Pagging;
using Repositories.Product;
using Services.DTO;

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

        public async Task<Package?> GetPackageByIdAsync(Guid id)
        {
            return await _packageRepository.GetPackageByIdAsync(id);
        }

        public async Task<List<Package>> GetPackageByTypeSell(string typeSell)
        {
            return await _packageRepository.GetPackageByTypeSell(typeSell);
        }

        public async Task<Package> AddPackageAsync(Package package)
        {
            return await _packageRepository.AddPackageAsync(package);
        }

        public async Task<Package?> UpdatePackageAsync(Guid id, UpdatePackageRequest updatePackageRequest)
        {
           var Package = await _packageRepository.GetPackageByIdAsync(id);
            if (Package == null)
            {
                throw new Exception("Package not found");
            }
            Package.CategoryId = updatePackageRequest.CategoryId;
            Package.PackageName = updatePackageRequest.PackageName;
            Package.PackagePrice = updatePackageRequest.PackagePrice;
            Package.Description = updatePackageRequest.Description;
            Package.PackageStatus = updatePackageRequest.PackageStatus;
            Package.TypeSell = updatePackageRequest.TypeSell;
           Package.Stock = updatePackageRequest.Stock;
            Package.Amount = updatePackageRequest.Amount;

             await _packageRepository.UpdatePackageAsync(Package);
            return Package;
        }

        public async Task<bool> DeletePackageAsync(Guid id)
        {
            return await _packageRepository.DeletePackageAsync(id);
        }

        public async Task<PaginatedList<Package>> GetAll(int pageNumber, int pageSize)
        {
            IQueryable<Package> packages = _packageRepository.GetAll().AsQueryable();
           
            return await PaginatedList<Package>.CreateAsync(packages, pageNumber, pageSize);
        }

        public async Task<IEnumerable<Package>> GetAllAsync(string? searchByCategory, string? searchByName, decimal? minPrice, decimal? maxPrice)
        {
            IQueryable<Package> packages = _packageRepository.GetAll().AsQueryable();

            if (!string.IsNullOrEmpty(searchByCategory))
            {
                packages = packages
                    .Include(b => b.Category)
                    .Where(b => b.Category.CategoryName.Contains(searchByCategory));
            }

            if (!string.IsNullOrEmpty(searchByName))
            {
                packages = packages.Where(b => b.PackageName.Contains(searchByName));
            }

            if (minPrice.HasValue)
            {
                packages = packages.Where(b => b.PackagePrice >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                packages = packages.Where(b => b.PackagePrice <= maxPrice.Value);
            }

            return await packages.ToListAsync();
        }

        public async Task<PaginatedList<Package>> GetAllFilter(string? typeSell,string? searchByCategory, string? searchByName, decimal? minPrice, decimal? maxPrice, int pageNumber, int pageSize)
        {
            IQueryable<Package> packages = _packageRepository.GetAll().AsQueryable();

            if (!string.IsNullOrEmpty(searchByCategory))
            {
                packages = packages
                    .Include(b => b.Category)
                    .Where(b => b.Category != null && b.Category.CategoryName.Contains(searchByCategory));
            }

            if (!string.IsNullOrEmpty(searchByName))
            {
                packages = packages.Where(b => b.PackageName.Contains(searchByName));
            }

            if (minPrice.HasValue)
            {
                packages = packages.Where(b => b.PackagePrice >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                packages = packages.Where(b => b.PackagePrice <= maxPrice.Value);
            }
            if(!string.IsNullOrEmpty(typeSell))
            {
                packages = packages.Where(b => b.TypeSell.Contains(typeSell));
            }

            return await PaginatedList<Package>.CreateAsync(packages, pageNumber, pageSize);
        }

        public async Task<BlindBox> GetRandomBlindBoxFromPackage(Guid packageId)
        {
            var package = await _packageRepository.GetPackageByIdAsync(packageId);
            if (package == null || package.BlindBoxes == null || !package.BlindBoxes.Any())
                throw new Exception("Package not found or contains no BlindBox.");

            // Chọn ngẫu nhiên một BlindBox từ danh sách
            var random = new Random();
            int index = random.Next(package.BlindBoxes.Count);

            return package.BlindBoxes.ElementAt(index);
        }


        // update package v2 danh cho hoang 
        public async Task<Package?> UpdatePackageAsyncV2(Guid id, UpdatePackageDTO updatePackageDTO)
        {
          
            var Package = await _packageRepository.GetPackageByIdAsync(id);
            if (Package == null)
            {
                throw new Exception("Package not found");
            }
           
            Package.PackageName = updatePackageDTO.PackageName;
            Package.PackagePrice = updatePackageDTO.PackagePrice;
            Package.Description = updatePackageDTO.Description;
            Package.PackageStatus = updatePackageDTO.PackageStatus;
            Package.TypeSell = updatePackageDTO.TypeSell;
            Package.Stock = updatePackageDTO.Stock;
            Package.Amount = updatePackageDTO.Amount;

            await _packageRepository.UpdatePackageAsync(Package);
            return Package;
            //bubu
        }
    }
}
