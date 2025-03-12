using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Package
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid PackageId { get; set; } = Guid.NewGuid();

        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        [StringLength(50)]
        public string PackageStatus { get; set; }

        public virtual Category? Category { get; set; }

        public virtual ICollection<PackageImage>? Images { get; set; }

        public virtual Cart? Cart { get; set; }

       
    }
}
