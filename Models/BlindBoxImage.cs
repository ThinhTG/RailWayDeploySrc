using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class BlindBoxImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid BlindBoxImageId { get; set; } = Guid.NewGuid();

        [Required]
        public Guid BlindBoxId { get; set; }

        [Required]
        public int DisplayBlindboxId { get; set; }

        [Required]
        [StringLength(255)]
        public string ImageUrl { get; set; }

        [Required]
        [ForeignKey("BlindBoxId")]
        public virtual BlindBox? BlindBox { get; set; }

    }
}
