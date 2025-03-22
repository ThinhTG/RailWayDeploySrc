using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
   public class AddCategoryDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CategoryId { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(255)]
        public string CategoryName { get; set; }

        public string CategoryImage { get; set; } = "";

        [Required]
        public DateTime CreatedAt { get; set; }


        [Required]
        public DateTime UpdatedAt { get; set; }
    }
}
