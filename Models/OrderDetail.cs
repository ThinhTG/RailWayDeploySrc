using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("OrderDetail")]
    public class OrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid OrderDetailId { get; set; } = Guid.NewGuid();

        [Required]
        public int OrderId { get; set; }

        [Required]
        public Guid? BlindBoxId { get; set; }

        [Required]
        public Guid? PackageId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        public virtual BlindBox? BlindBox { get; set; }

        public virtual Package? Package { get; set; }

        public virtual Order? Order { get; set; }

    }
}
