using Models;

namespace Services.DTO
{
    public class PackageImageDTO
    {
        public Guid PackageId { get; set; }
        public List<string>? ImageUrl { get; set; }

        public PackageImageDTO(Guid packageId, List<string> imageUrl)
        {
            PackageId = packageId;
            ImageUrl = imageUrl;
        }
    }
}
