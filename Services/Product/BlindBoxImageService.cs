using Models;
using Repositories.UnitOfWork;
using Services.DTO;

namespace Services.Product
{
    public class BlindBoxImageService : IBlindBoxImageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BlindBoxImageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddBlindBoxImages(BBImageDTO blindBoxImage)
        {
            if (blindBoxImage.BlindBoxId == Guid.Empty)
            {
                throw new Exception("BlindBoxId is required");
            }
            if (blindBoxImage.ImageUrls == null || !blindBoxImage.ImageUrls.Any())
            {
                throw new Exception("At least one Image URL is required");
            }

            var blindboxImageRepo = _unitOfWork.GetRepository<BlindBoxImage>();
            var blindbox = _unitOfWork.GetRepository<BlindBox>();

            var blindBox = blindbox.GetById(blindBoxImage.BlindBoxId);
            if (blindBox == null)
            {
                throw new Exception("BlindBox not found");
            }

            var newBlindBoxImages = blindBoxImage.ImageUrls.Select((imageUrl, index) => new BlindBoxImage
            {
                BlindBoxId = blindBoxImage.BlindBoxId,
                BlindBoxImageId = Guid.NewGuid(),
                ImageUrl = imageUrl,
                DisplayBlindboxId = index == 0 ? 1 : 0, // Set DisplayBlindboxId to 1 for the first image, 0 for others
                BlindBox = blindBox
            }).ToList();

            blindboxImageRepo.InsertRange(newBlindBoxImages);

            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<BlindBoxImage>> GetBlindBoxImages(Guid blindboxImageId)
        {
            var blindboxImageRepo = _unitOfWork.GetRepository<BlindBoxImage>();
            var blindboxRepo = _unitOfWork.GetRepository<BlindBox>();

            var blindboxImages = await blindboxImageRepo.FindListAsync(b => b.BlindBoxImageId == blindboxImageId);
            //var blindboxImages = await blindboxImageRepo.Entities.Where(b => b.BlindBoxImageId == blindboxImageId).Select(b => b.BlindBox).FirstOrDefaultAsync();

            /*var blindboxs = await blindboxRepo.FindListAsync(b => blindboxImages.Contains());*/

            if (blindboxImages == null)
            {
                throw new Exception("error get blindbox image");
            }
            else
            {

                return blindboxImages;
            }
            //var blindboxImages = await blindboxImageRepo.FindAllAsync(b => b.BlindBoxId == blindboxId);

        }

        //get list image by blindboxId
        public async Task<IEnumerable<BlindBoxImage>> GetBlindBoxImageByBlindBox(Guid blindboxId)
        {
            var blindboxImageRepo = _unitOfWork.GetRepository<BlindBoxImage>();
            var blindboxImages = await blindboxImageRepo.FindListAsync(b => b.BlindBoxId == blindboxId);
            if (blindboxImages == null)
            {
                throw new Exception("error get blindbox image");
            }
            else
            {
                return blindboxImages;
            }
        }

        public async Task<bool> UpdateBlindBoxImage(Guid blindboxImageId, string imageURL)
        {
            if (imageURL is null)
            {
                throw new Exception("Image URL is required");
            }

            var blindboxImageRepo = _unitOfWork.GetRepository<BlindBoxImage>();

            var blindboxImage = await blindboxImageRepo.GetByIdAsync(blindboxImageId);
            if (blindboxImage == null)
            {
                throw new Exception("BlindBoxImage not found");
            }

            try
            {
                blindboxImage.ImageUrl = imageURL;
                blindboxImageRepo.Update(blindboxImage);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> DeleteBlindBoxImage(Guid blindboxImageId)
        {

            var blindboxImageRepo = _unitOfWork.GetRepository<BlindBoxImage>();

            var blindboxImage = await blindboxImageRepo.GetByIdAsync(blindboxImageId);
            if (blindboxImage == null)
            {
                throw new Exception("BlindBoxImage not found");
            }

            try
            {
                blindboxImageRepo.Delete(blindboxImage);
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
