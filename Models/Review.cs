using System.Text.Json.Serialization;

namespace Models
{
    public class Review
    {
        public Guid ReviewId { get; set; } = Guid.NewGuid();
        public Guid OrderDetailId { get; set; }
        public string AccountId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
        public string ReviewStatus { get; set; } = "Pending";
        // Quan hệ navigation

        [JsonIgnore]
        public virtual OrderDetail OrderDetail { get; set; }
        [JsonIgnore]
        public virtual ApplicationUser Account { get; set; }

    }
  

}
