using Models;
using Services.DTO;

namespace Services.Product
{
    public interface IPackageImageService
    {
        Task AddPackageImage(PackageImageDTO packgeImageDTO);
        Task<IEnumerable<PackageImage>> GetPackageImages(Guid packageId);
        Task<bool> UpdatePackageImage(Guid packageimageId, string imageURL);
        Task<bool> DeletePackageImage(Guid packageimageId);
        Task<Package?> FindPackageByImageId(Guid packageImageId);
        Task<IEnumerable<PackageImage>> GetPackageImagesByPackageId(Guid packageId);
    }
}
