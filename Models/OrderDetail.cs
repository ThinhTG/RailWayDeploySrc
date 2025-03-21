using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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

        public Guid? BlindBoxId { get; set; }

        public Guid? PackageId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }


        [JsonIgnore]
        public virtual BlindBox? BlindBox { get; set; }
        [JsonIgnore]
        public virtual Package? Package { get; set; }
        [JsonIgnore]
        public virtual Order? Order { get; set; }




    }
}
