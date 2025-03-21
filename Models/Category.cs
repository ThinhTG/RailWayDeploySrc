using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CategoryId { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(255)]
        public string CategoryName { get; set; }

        [Required]
        public string CategoryImage { get; set; } = null;

        [Required]
        public string TypeSell { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        public virtual ICollection<Package>? Packages { get; set; }

        public virtual ICollection<BlindBox>? BlindBoxes { get; set; }

    }
}
