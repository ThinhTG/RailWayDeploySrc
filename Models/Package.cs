﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [StringLength(255)]
        public string PackageName { get; set; }

        [Required]
        [Column(TypeName = "decimal(19,0)")]
        public decimal PackagePrice { get; set; }

        [StringLength(255)]
        public string? Description { get; set; } // Cho phép null

        [Required]
        public int Stock { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        [StringLength(50)]
        public string PackageStatus { get; set; }

        public virtual Category? Category { get; set; }

        public virtual ICollection<PackageImage>? Images { get; set; }

        public virtual Cart? Cart { get; set; }

        public virtual ICollection<BlindBox>? BlindBoxes { get; set; }

        public Guid? OrderDetailId { get; set; }
        public virtual ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}
