using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public int DisplayPackageId { get; set; }

        [Required]
        [StringLength(255)]
        public string ImageUrl { get; set; }

        [Required]
        [ForeignKey("PackageId")]
        public virtual Package? Package { get; set; }
    }
}
