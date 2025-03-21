using Models;
using System.ComponentModel.DataAnnotations;

public class Cart
{
    [Key]
    public Guid CartId { get; set; } = Guid.NewGuid();

    public Guid? BlindBoxId { get; set; }
    public virtual BlindBox? BlindBox { get; set; }

    public Guid? PackageId { get; set; }
    public virtual Package? Package { get; set; }

    public int Quantity { get; set; }

    public DateTime CreateDate { get; set; } = DateTime.UtcNow;

    public string UserId { get; set; }
    public virtual ApplicationUser? applicationUser { get; set; }
}
