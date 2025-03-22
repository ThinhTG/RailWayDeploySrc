using DAO.Contracts;
using Models;
using Repositories.OrderRep;
using Services.DTO;
using Services.Product;
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
        private readonly IBlindBoxService _blindBoxService;
        private readonly IPackageService _packageService;
        public OrderDetailService(IOrderDetailRepository orderDetailRepository, IPackageService packageService, IBlindBoxService blindBoxService)
        {
            _orderDetailRepository = orderDetailRepository;
            _packageService = packageService;
            _blindBoxService = blindBoxService;
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

        public async Task<IEnumerable<OrderDetail>> GetOrderDetailsByOrderIdAsync(int orderId)
        {
            return await _orderDetailRepository.GetOrderDetailsByOrderIdAsync(orderId);
        }

        public async Task<IEnumerable<OrderDetail>> AddMultipleOrderDetailsAsync(int orderId, List<OrderDetailRq> orderDetails)
        {
            var createdOrderDetails = new List<OrderDetail>();

            foreach (var orderDetailRequest in orderDetails)
            {
                // Validate BlindBoxId (if provided)
                if (orderDetailRequest.BlindBoxId.HasValue)
                {
                    var blindBoxExists = await _blindBoxService.GetByIdAsync(orderDetailRequest.BlindBoxId.Value);
                    if (blindBoxExists == null)
                    {
                        throw new Exception($"BlindBox with BlindBoxId {orderDetailRequest.BlindBoxId} does not exist.");
                    }
                }

                // Validate PackageId (if provided)
                if (orderDetailRequest.PackageId.HasValue)
                {
                    var packageExists = await _packageService.GetPackageByIdAsync(orderDetailRequest.PackageId.Value);
                    if (packageExists == null)
                    {
                        throw new Exception($"Package with PackageId {orderDetailRequest.PackageId} does not exist.");
                    }
                }

                var orderDetail = new OrderDetail
                {
                    OrderId = orderId,
                    PackageId = orderDetailRequest.PackageId,
                    BlindBoxId = orderDetailRequest.BlindBoxId,
                    Quantity = orderDetailRequest.Quantity,
                    Price = orderDetailRequest.Price,
                };

                createdOrderDetails.Add(orderDetail);
            }

            // Lưu vào Repository
            await _orderDetailRepository.AddMultipleOrderDetailsAsync(createdOrderDetails);

            return createdOrderDetails;
        }

       
    }
}