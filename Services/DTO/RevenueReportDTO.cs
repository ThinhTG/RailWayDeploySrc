using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class RevenueReportDTO
    {
        public DateTime Date { get; set; }  // Ngày/tháng/năm
        public decimal TotalRevenue { get; set; } // Tổng doanh thu
        public int TotalOrders { get; set; } // Tổng số đơn hàng
    }
}
