using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class AddVoucherDTO
    {
        private static Random random = new Random();

        [Required]
        public int Quantity { get; set; }

        [Required]
        public string VoucherCode = GenerateVoucherCode();

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

        private static string GenerateVoucherCode()
        {
            const string prefix = "BBS-";
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return prefix + new string(Enumerable.Repeat(chars, 4)
                .Select(s => s[random.Next(s.Length)]).ToArray()) + random.Next(1000, 10000);
        }
    }
}
