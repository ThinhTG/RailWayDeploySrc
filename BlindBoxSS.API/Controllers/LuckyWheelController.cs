using BlindBoxSS.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.OrderS;
using Services.Product;
using Services.Request;
using Services.DTO;
using Services.Wallet;

namespace BlindBoxSS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LuckyWheelController : ControllerBase
    {
        private readonly IPackageService _packageService;
        private readonly IWalletService _walletService;
        private readonly IOrderService _orderService;
        private readonly IOrderDetailService _orderDetailService;
        private readonly IBlindBoxService _blindBoxService;
        private readonly IWalletTransactionService _walletTransaction;

        public LuckyWheelController(IPackageService packageService, IWalletService walletService, IOrderService orderService, IOrderDetailService orderDetailService,IBlindBoxService blindBoxService, IWalletTransactionService transactionService)
        {
            _packageService = packageService;
            _walletService = walletService;
            _orderService = orderService;
            _orderDetailService = orderDetailService;
            _blindBoxService = blindBoxService;
            _walletTransaction = transactionService;
        }

        [HttpPost("spin")]
        public async Task<IActionResult> SpinWheel([FromBody] SpinRequest request)
        {
            var package = await _packageService.GetPackageByIdAsync(request.PackageId);
            if (package == null || package.Stock <= 0)
                return BadRequest("Package not available or out of stock.");

            var newRequestAccountId = Guid.Parse(request.AccountId);
            var userWallet = await _walletService.GetWalletByAccountId(newRequestAccountId);
            if (userWallet == null || userWallet.Balance < package.PackagePrice / 10)
                return BadRequest("Insufficient balance.");

            // Trừ tiền trong ví
            decimal spinCost = package.PackagePrice / 10;
            userWallet.Balance -= spinCost;
            await _walletService.UpdateUserWalletAsync(userWallet);

            var blindBox = await _packageService.GetRandomBlindBoxFromPackage(request.PackageId);
            if (blindBox == null)
                return BadRequest("No BlindBox available in package.");

            // Chuyển trạng thái BlindBox thành "Sold"
            blindBox.BlindBoxStatus = "SoldOut";
            await _blindBoxService.UpdateAsync(blindBox.BlindBoxId, new UpdateBlindBoxDTO
            {
                BlindBoxStatus = blindBox.BlindBoxStatus
            });

            // Tạo đơn hàng
            var order = new Order
            {
                AccountId = request.AccountId,
                OrderStatus = Models.Enum.OrderStatus.PENDING,
                Price = spinCost,
                PriceTotal = spinCost,
                CreatedDate = DateTime.UtcNow,
                PaymentConfirmed = true, 
                Note = "Spin reward",
                PhoneNumber = "",
                DiscountMoney = 0
            };

            var createdOrder = await _orderService.AddAsync(order);

            var orderDetail = new OrderDetail
            {
                OrderId = createdOrder.OrderId,
                BlindBoxId = blindBox.BlindBoxId,
                Quantity = 1,
                Price = spinCost
            };

            await _orderDetailService.AddOrderDetailAsync(orderDetail);

            return Ok(new { Message = "Spin successful!", BlindBox = blindBox, Balance = userWallet.Balance });
        }
    }

}

