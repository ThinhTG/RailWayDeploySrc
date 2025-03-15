using Models;
using Repositories.OrderRep;
using Repositories.Pagging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.OrderS
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<PaginatedList<Order>> GetAll(int pageNumber, int pageSize)
        {
            IQueryable<Order> orders = _orderRepository.GetAll().AsQueryable();
            return await PaginatedList<Order>.CreateAsync(orders, pageNumber, pageSize);
        }
        public async Task<Order> AddAsync(Order order)
        {
            return await _orderRepository.AddAsync(order);
        }

        public async Task DeleteAsync(int id)
        {
            await _orderRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _orderRepository.GetOrdersAsync();
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            return await _orderRepository.GetByIdAsync(id);
        }

        public async Task<Order> UpdateAsync(Order order)
        {
            return await _orderRepository.UpdateAsync(order);
        }

        public async Task<PaginatedList<Order>> GetByAccountId(string accountId, int pageNumber, int pageSize)
        {
            IQueryable<Order> orders = _orderRepository.GetByAccountId(accountId).AsQueryable();

            return await PaginatedList<Order>.CreateAsync(orders, pageNumber, pageSize);
        }
    }
}