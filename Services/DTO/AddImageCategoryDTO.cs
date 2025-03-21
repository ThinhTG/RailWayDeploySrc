using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
   public class AddImageCategoryDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CategoryId { get; set; }

        [Required]
        public string CategoryImage { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
