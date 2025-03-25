using Models;
using Repositories.UnitOfWork;
using Services.DTO;

namespace Services.Product
{
    public class PackageImageService : IPackageImageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PackageImageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddPackageImages(PackageImageDTO packageImageDTO)
        {
            if (packageImageDTO.PackageId == null)
            {
                throw new Exception("PackageId is required");
            }
            if (packageImageDTO.ImageUrl == null || !packageImageDTO.ImageUrl.Any())
            {
                throw new Exception("At least one Image URL is required");
            }

            var packageImageRepo = _unitOfWork.GetRepository<PackageImage>();
            var package = _unitOfWork.GetRepository<Package>();
            var Packages = package.GetById(packageImageDTO.PackageId);

            //var newPackageImages = packageImageDTO.ImageUrl.Select((imageUrl,index) => new PackageImage
            //{
            //    PackageId = packageImageDTO.PackageId,
            //    PackageImageId = Guid.NewGuid(),
            //    DisplayPackageId = index == 0 ? 1 : 0, // Set DisplayBlindboxId to 1 for the first image, 0 for others
            //    ImageUrl = imageUrl
            //}).ToList();
            var newPackageImages = packageImageDTO.ImageUrl.Select((imageUrl, index) => new PackageImage
            {
                PackageId = packageImageDTO.PackageId,
                PackageImageId = Guid.NewGuid(),
                ImageUrl = imageUrl,
                DisplayPackageId = index == 0 ? 1 : 0, // Set DisplayBlindboxId to 1 for the first image, 0 for others
                Package = Packages
            }).ToList();

            packageImageRepo.InsertRange(newPackageImages);

            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<PackageImage>> GetPackageImages(Guid packageImageId)
        {


            var packageImageRepo = _unitOfWork.GetRepository<PackageImage>();

            var packageImages = await packageImageRepo.FindAllAsync(b => b.PackageImageId == packageImageId);
            //var packageImages = await packageImageRepo.FindAllAsync(b => b.PackageImageId == packageImageId).Where(b => b.PackageImageId == packageImageId).Select(b => b.Package).FirstOrDefaultAsync();

            if (packageImages == null)
            {
                throw new Exception("error get package image");
            }
            else
            {
                return packageImages;
            }
        }

        //get list package image by packageId
        public async Task<IEnumerable<PackageImage>> GetPackageImagesByPackageId(Guid packageId)
        {
            var packageImageRepo = _unitOfWork.GetRepository<PackageImage>();
            var packageImages = await packageImageRepo.FindListAsync(b => b.PackageId == packageId);
            if (packageImages == null)
            {
                throw new Exception("error get package image");
            }
            else
            {
                return packageImages;
            }
        }

        public async Task<Package?> FindPackageByImageId(Guid packageImageId)
        {
            var packageImage = await _unitOfWork.GetRepository<PackageImage>().FindOneAsync(pi => pi.PackageImageId == packageImageId);
            if (packageImage == null)
            {
                throw new Exception("PackageImage not found");
            }

            return packageImage.Package;
        }

        public async Task<bool> UpdatePackageImage(Guid packageImageId, string imageURL)
        {
            if (imageURL is null)
            {
                throw new Exception("Image URL is required");
            }

            var packageImageRepo = _unitOfWork.GetRepository<PackageImage>();

            var packageImage = await packageImageRepo.GetByIdAsync(packageImageId);
            if (packageImage == null)
            {
                throw new Exception("packageImage not found");
            }

            try
            {
                packageImage.ImageUrl = imageURL;
                packageImageRepo.Update(packageImage);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> DeletePackageImage(Guid packageImageId)
        {

            var packageImageRepo = _unitOfWork.GetRepository<PackageImage>();

            var packageImage = await packageImageRepo.GetByIdAsync(packageImageId);
            if (packageImage == null)
            {
                throw new Exception("packageImage not found");
            }

            try
            {
                packageImageRepo.Delete(packageImage);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
