using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class VoucherResponse
    {
        public Guid VoucherId { get; set; }
        public string Description { get; set; }
        public decimal DiscountMoney { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime EndDate { get; set; }
    }
}
