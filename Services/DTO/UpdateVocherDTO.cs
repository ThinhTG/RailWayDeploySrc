using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class UpdateVoucherDTO
    {
      

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal DiscountMoney { get; set; }

        [Required]
        public decimal Money { get; set; }

        [Required]
        public DateTime StartDate { get; set; } = DateTime.Now;

        [Required]
        public DateTime EndDate { get; set; }

       
    }
}
