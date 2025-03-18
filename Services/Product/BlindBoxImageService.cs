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

        public async Task AddBlindBoxImage(BBImageDTO blindBoxImage)
        {
            if (blindBoxImage.BlindBoxId == null)
            {
                throw new Exception("BlindBoxId is required");
            }
            if (blindBoxImage.ImageUrl is null)
            {
                throw new Exception("Image URL is required");
            }
            var blindboxImageRepo = _unitOfWork.GetRepository<BlindBoxImage>();
            var newBlindBoxImage = new BlindBoxImage
            {
                BlindBoxId = blindBoxImage.BlindBoxId,
                BlindBoxImageId = Guid.NewGuid(),
                ImageUrl = blindBoxImage.ImageUrl
            };

            blindboxImageRepo.Insert(newBlindBoxImage);

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
