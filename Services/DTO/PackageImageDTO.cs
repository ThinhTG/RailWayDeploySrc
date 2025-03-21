namespace Services.DTO
{
    public class PackageImageDTO
    {
        public Guid PackageId { get; set; }
        public string? ImageUrl { get; set; }
        public int DisplayPackageId { get; set; }
    }
}
