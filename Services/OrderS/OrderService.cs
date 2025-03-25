using Models;
using Models.Enum;
using Repositories.OrderRep;
using Repositories.Pagging;
using Repositories.Product;
using Services.AccountService;
using Services.AddressS;
using Services.DTO;
using Services.Payment;

namespace Services.OrderS
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IBlindBoxRepository _blindBoxRepository;
        private readonly IPackageRepository _packageRepository;
        private readonly IAccountService _accountService;
        private readonly IAddressService _addressService;
        private readonly ICartService _cartService;
        private readonly Lazy<IPaymentService> _paymentService;

        public OrderService(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, IAccountService accountService, IAddressService addressService, Lazy<IPaymentService> paymentService, IPackageRepository packageRepository, IBlindBoxRepository blindBoxRepository, ICartService cartService)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _accountService = accountService;
            _addressService = addressService;
            _paymentService = paymentService;
            _packageRepository = packageRepository;
            _blindBoxRepository = blindBoxRepository;
            _cartService = cartService;
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


        // daonh thu theo ngay
        public List<RevenueReportDTO> GetRevenueByDay(DateTime startDate, DateTime endDate)
        {
            var orders = _orderRepository.GetOrdersByDateRange(startDate, endDate);
            return orders
                .GroupBy(o => o.CreatedDate.Date)
                .Select(g => new RevenueReportDTO
                {
                    Date = g.Key,
                    TotalRevenue = g.Sum(o => o.PriceTotal),
                    TotalOrders = g.Count()
                })
                .OrderBy(r => r.Date)
                .ToList();
        }


        // doanh thu theo thang
        public List<RevenueReportDTO> GetRevenueByMonth(int year)
        {
            var orders = _orderRepository.GetOrdersByYear(year);
            return orders
                .GroupBy(o => new { o.CreatedDate.Year, o.CreatedDate.Month })
                .Select(g => new RevenueReportDTO
                {
                    Date = new DateTime(g.Key.Year, g.Key.Month, 1),
                    TotalRevenue = g.Sum(o => o.PriceTotal),
                    TotalOrders = g.Count()
                })
                .OrderBy(r => r.Date)
                .ToList();
        }


        // doanh thu theo nam
        public List<RevenueReportDTO> GetRevenueByYear()
        {
            var orders = _orderRepository.GetAllConfirmedOrders();
            return orders
                .GroupBy(o => o.CreatedDate.Year)
                .Select(g => new RevenueReportDTO
                {
                    Date = new DateTime(g.Key, 1, 1),
                    TotalRevenue = g.Sum(o => o.PriceTotal),
                    TotalOrders = g.Count()
                })
                .OrderBy(r => r.Date)
                .ToList();
        }


        //update paymentConfirmed by param orderCode
        public async Task<Order> UpdatePaymentConfirmed(int? orderCode, int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);

            if (order == null)
            {
                throw new KeyNotFoundException($"Order with ID {orderId} not found.");
            }
            var orderDetail = await _orderDetailRepository.GetOrderDetailsByOrderIdAsync(orderId);
            var card = await _cartService.GetCartByUserId(order.AccountId);

            var blindbox = await _blindBoxRepository.GetByIdAsync(orderDetail.First().BlindBoxId);
            //if (blindbox == null)
            //{
            //    throw new KeyNotFoundException($"BlindBox with ID {orderDetail.First().BlindBoxId} not found.");
            //}
            var package = await _packageRepository.GetPackageByIdAsync(orderDetail.First().PackageId);
            //if (package == null)
            //{
            //    throw new KeyNotFoundException($"Package with ID {orderDetail.First().PackageId} not found.");
            //}


            // if have orderCode
            if (orderCode != null)
            {
                var payment = await _paymentService.Value.GetPaymentLinkInformationAsync(orderCode.Value);
                if (payment.status == "PAID")
                {
                    order.PaymentConfirmed = true;
                    //get all quantity of blindbox in order
                    if (blindbox != null)
                    {
                        var quantityBB = orderDetail.Sum(o => o.Quantity);
                        blindbox.Stock = blindbox.Stock - quantityBB;
                        await _blindBoxRepository.UpdateAsync(blindbox);
                        foreach (var item in card)
                        {
                            if (item.BlindBoxId == blindbox.BlindBoxId)
                            {
                                await _cartService.DeleteCartItem(item.CartId);
                            }
                        }
                    }
                    if (package != null)
                    {
                        var quantityPackage = orderDetail.Sum(o => o.Quantity);
                        package.Stock = package.Stock - quantityPackage;
                        await _packageRepository.UpdatePackageAsync(package);
                        foreach (var item in card)
                        {
                            if (item.PackageId == package.PackageId)
                            {
                                await _cartService.DeleteCartItem(item.CartId);
                            }
                        }
                    }
                    return await _orderRepository.UpdateAsync(order);

                }
                return order;
            }
            if (blindbox != null)
            {
                var quantityBB = orderDetail.Sum(o => o.Quantity);
                blindbox.Stock = blindbox.Stock - quantityBB;
                await _blindBoxRepository.UpdateAsync(blindbox);
                foreach (var item in card)
                {
                    if (item.BlindBoxId == blindbox.BlindBoxId)
                    {
                        await _cartService.DeleteCartItem(item.CartId);
                    }
                }
            }
            if (package != null)
            {
                var quantityPackage = orderDetail.Sum(o => o.Quantity);
                package.Stock = package.Stock - quantityPackage;
                await _packageRepository.UpdatePackageAsync(package);
                foreach (var item in card)
                {
                    if (item.PackageId == package.PackageId)
                    {
                        await _cartService.DeleteCartItem(item.CartId);
                    }
                }
            }
            order.PaymentConfirmed = true;
            return await _orderRepository.UpdateAsync(order);
        }

        public async Task<Order> UpdateOrderStatus(int orderId, OrderStatus orderStatus)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new KeyNotFoundException($"Order with ID {orderId} not found.");
            }
            order.OrderStatus = orderStatus;
            return await _orderRepository.UpdateAsync(order);

        }

        public async Task<PaginatedList<Order>> GetListOrderDelivering(int pageNumber, int pageSize)
        {
            IEnumerable<Order> orders = await _orderRepository.GetListOrderDelivering();
            return PaginatedList<Order>.Create(orders.ToList(), pageNumber, pageSize);
        }
        public async Task<PaginatedList<Order>> GetListOrderCompleted(int pageNumber, int pageSize)
        {
            IEnumerable<Order> orders = await _orderRepository.GetListOrderCompleted();
            return PaginatedList<Order>.Create(orders.ToList(), pageNumber, pageSize);
        }
    }
}