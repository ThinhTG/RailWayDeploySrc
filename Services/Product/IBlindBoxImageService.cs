using Models;
using Services.DTO;


namespace Services.Product
{
    public interface IBlindBoxImageService
    {
        Task AddBlindBoxImages(BBImageDTO blindBoxImage);
        Task<IEnumerable<BlindBoxImage>> GetBlindBoxImages(Guid blindboxId);
        Task<bool> UpdateBlindBoxImage(Guid blindboximageId, string imageURL);
        Task<bool> DeleteBlindBoxImage(Guid blindboximageId);
        Task<IEnumerable<BlindBoxImage>> GetBlindBoxImageByBlindBox(Guid blindboxId);
    }
}
