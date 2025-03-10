using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class BlindBox
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid BlindBoxId { get; set; } = Guid.NewGuid();

        [Required]
        public Guid PackageId { get; set; }

        [Required]
        [StringLength(255)]
        public string BlindBoxName { get; set; }

        [Required]
        [Column(TypeName = "decimal(19,0)")]
        public decimal Price { get; set; }

        [StringLength(255)]
        public string? Description { get; set; } // Cho phép null

        [Required]
        public int Stock { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        public float? Percent { get; set; } // Cho phép null

        [Required]
        [StringLength(50)]
        public string BlindBoxStatus { get; set; }
    }
}
