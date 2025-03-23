using Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Cart
{
    [Key]
    public Guid CartId { get; set; } = Guid.NewGuid();

    public Guid? BlindBoxId { get; set; }

    [JsonIgnore]
    public virtual BlindBox? BlindBox { get; set; }

    public Guid? PackageId { get; set; }
    [JsonIgnore]
    public virtual Package? Package { get; set; }

    public int Quantity { get; set; }

    public DateTime CreateDate { get; set; } = DateTime.UtcNow;

    public string UserId { get; set; }
    public virtual ApplicationUser? applicationUser { get; set; }
}
