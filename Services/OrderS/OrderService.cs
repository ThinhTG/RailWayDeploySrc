using Models;
using Repositories.OrderRep;
using Repositories.Pagging;
using Services.AccountService;
using Services.AddressS;
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
        private readonly IAccountService _accountService;
        private readonly IAddressService _addressService;

        public OrderService(IOrderRepository orderRepository, IAccountService accountService, IAddressService addressService)
        {
            _orderRepository = orderRepository;
            _accountService = accountService;
            _addressService = addressService;
        }

        public async Task<PaginatedList<Order>> GetAll(int pageNumber, int pageSize)
        {
            IQueryable<Order> orders = _orderRepository.GetAll().AsQueryable();
            return await PaginatedList<Order>.CreateAsync(orders, pageNumber, pageSize);
        }
        public async Task<Order> AddAsync(Order order)
        {
            var account = await _accountService.GetByIdAsync(order.AccountId);
            if (account == null)
            {
                throw new KeyNotFoundException($"Account with ID {order.AccountId} not found.");
            }

            var address = await _addressService.GetByIdAsync(order.AddressId);

            if (address == null)
            {
                throw new KeyNotFoundException($"Address with ID {order.AddressId} not found.");
            }

            order.PhoneNumber = address.PhoneNumber;


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

        public async Task<PaginatedList<Order>> GetListOrderForCheck(int pageNumber, int pageSize)
        {
            IQueryable<Order> orders = _orderRepository.GetListOrderForCheck().AsQueryable();

            return await PaginatedList<Order>.CreateAsync(orders, pageNumber, pageSize);
        }
    }
}