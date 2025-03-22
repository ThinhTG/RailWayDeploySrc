using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Enum
{
    public enum OrderStatus
    {
        WaitingForConfirmation, // Chờ xác nhận
        WaitingForPickup,       // Chờ lấy hàng
        WaitingForDelivery,     // Chờ giao hàng
        InDelivery,             // Đang giao hàng
        OrderReceived,          // Đã nhận hàng
        Completed,              // Hoàn thành
        OrderCanceled           // Đã hủy
    }
}
