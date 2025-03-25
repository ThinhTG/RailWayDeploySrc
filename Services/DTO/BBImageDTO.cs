namespace Services.DTO
{
    public class BBImageDTO
    {
        public Guid BlindBoxId { get; set; }
        public List<string>? ImageUrls { get; set; }
        //public int DisplayBlindboxId { get; set; }

        public BBImageDTO(Guid blindBoxId, List<string> imageUrls)
        {
            BlindBoxId = blindBoxId;
            ImageUrls = imageUrls;
            //DisplayBlindboxId = imageUrls != null && imageUrls.Count > 0 ? 0 : -1;
        }
    }
}
