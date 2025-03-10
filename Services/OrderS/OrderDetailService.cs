using Models;
using Repositories.OrderRep;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.OrderS
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IOrderDetailRepository _orderDetailRepository;
        public OrderDetailService(IOrderDetailRepository orderDetailRepository)
        {
            _orderDetailRepository = orderDetailRepository;
        }
        public async Task<IEnumerable<OrderDetail>> GetAllOrderDetailAsync()
        {
            return await _orderDetailRepository.GetAllOrderDetailAsync();
        }

        public async Task<OrderDetail?> GetOrderDetailByIdAsync(Guid id)
        {
            return await _orderDetailRepository.GetOrderDetailByIdAsync(id);
        }

        public async Task<OrderDetail> AddOrderDetailAsync(OrderDetail orderDetail)
        {
            return await _orderDetailRepository.AddOrderDetailAsync(orderDetail);
        }

        public async Task<bool> DeleteOrderDetailAsync(Guid id)
        {
            return await _orderDetailRepository.DeleteOrderDetailAsync(id);
        }


        public async Task<OrderDetail?> UpdateOrderDetailAsync(OrderDetail orderDetail)
        {
            return await _orderDetailRepository.UpdateOrderDetailAsync(orderDetail);

        }
    }
}