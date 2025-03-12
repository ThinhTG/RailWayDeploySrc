using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class PackageImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid PackageImageId { get; set; }

        [Required]
        public Guid PackageId { get; set; }

        [Required]
        [StringLength(255)]
        public string ImageUrl { get; set; }

        public virtual Package? Package { get; set; }
    }
}
