using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.OrderS
{
    public interface IOrderDetailService
    {
        Task<IEnumerable<OrderDetail>> GetAllOrderDetailAsync();
        Task<OrderDetail?> GetOrderDetailByIdAsync(Guid id);
        Task<OrderDetail> AddOrderDetailAsync(OrderDetail orderDetail);
        Task<OrderDetail?> UpdateOrderDetailAsync(OrderDetail orderDetail);
        Task<bool> DeleteOrderDetailAsync(Guid id);

        Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId);
    }
}