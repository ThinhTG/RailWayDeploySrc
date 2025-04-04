﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Models
{
    public class BlindBox
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid BlindBoxId { get; set; } = Guid.NewGuid();

        [Required]
        public Guid PackageId { get; set; }

        [JsonIgnore]
        public virtual Package Package { get; set; }    


        [Required]
        public Guid CategoryId { get; set; }

        public virtual Category Category { get; set; }

        [Required]
        [StringLength(255)]
        public string BlindBoxName { get; set; }

        [Required]
        public string TypeSell { get; set; }

        [Required]
        public string Size { get; set; }

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

        public bool isSecret { get; set; }

        [JsonIgnore]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        public virtual ICollection<BlindBoxImage> BlindBoxImages { get; set; } = new List<BlindBoxImage>();
    }
}
