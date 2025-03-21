using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class CreatePackageRequest
    {
        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        [StringLength(255)]
        public string PackageName { get; set; }

        [Required]
        public string TypeSell { get; set; }

        [Required]
        [Column(TypeName = "decimal(19,0)")]
        public decimal PackagePrice { get; set; }

        [StringLength(255)]
        public string? Description { get; set; } // Cho phép null

        [Required]
        public int Stock { get; set; }

        [Required]
        public int Amount { get; set; } = 0;

        [Required]
        [StringLength(50)]
        public string PackageStatus { get; set; }
    }
}
