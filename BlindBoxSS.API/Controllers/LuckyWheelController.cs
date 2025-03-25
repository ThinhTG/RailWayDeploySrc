using BlindBoxSS.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.OrderS;
using Services.Product;
using Services.Request;
using Services.DTO;
using Services.Wallet;
using Services.AddressS;
using System.Transactions;
using TimeZoneConverter;
using System.Globalization;

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
        private readonly IAddressService _addressService;
        private readonly IConfiguration _configuration;

        public LuckyWheelController(IPackageService packageService, IWalletService walletService, IOrderService orderService, IOrderDetailService orderDetailService,IBlindBoxService blindBoxService, IWalletTransactionService transactionService, IAddressService addressService, IConfiguration configuration)
        {
            _packageService = packageService;
            _walletService = walletService;
            _orderService = orderService;
            _orderDetailService = orderDetailService;
            _blindBoxService = blindBoxService;
            _walletTransaction = transactionService;
            _addressService = addressService;
            _configuration = configuration;
        }

        /// <param name="pageNumber">số trang</param>
        /// <param name="pageSize">số Blindbox</param>
        [HttpGet("packages/active")]
        public async Task<IActionResult> GetActivePackages(int pageNumber = 1, int pageSize = 5)
        {
            var packages = await _packageService.GetActiveLWPackagesPaged(pageNumber, pageSize);
            return Ok(packages);
        }

        /*
        [HttpPost("spin")]
        public async Task<IActionResult> SpinWheel([FromBody] SpinRequest request)
        {
            var dateFormat = _configuration["TransactionSettings:DateFormat"] ?? "yyyy-MM-ddTHH:mm:ssZ";
            bool useUTC = bool.TryParse(_configuration["TransactionSettings:UseUTC"], out bool utc) && utc;
            var timeZoneId = _configuration["TransactionSettings:TimeZone"] ?? "UTC";
            DateTime transactionDatetime = DateTime.UtcNow; // Default to UTC

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

            if(package.Amount == 1) {
            // Chuyển trạng thái Package thành "SoldOut"
            await _packageService.UpdateStatusAsync(package.PackageId, "SoldOut");
            }

            await _blindBoxService.UpdateStatusAsync(blindBox.BlindBoxId, "SoldOut");

            if (package.Amount >= 1)
            {

                var newUpdatePackage = new UpdatePackageRequest
                {
                    CategoryId = package.CategoryId,
                    PackageName = package.PackageName,
                    PackagePrice = package.PackagePrice,
                    Amount = package.Amount - 1,
                    Description = package.Description,
                    Stock = package.Stock,
                    PackageStatus = package.PackageStatus,
                    TypeSell = package.TypeSell,
                };
                await _packageService.UpdatePackageAsync(request.PackageId, newUpdatePackage);
            }

            var address = await _addressService.GetDefaultAddressByAccoutId(request.AccountId);
        
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
                DiscountMoney = 0,
                AddressId = address.AddressId
            };

            var createdOrder = await _orderService.AddAsync(order);

            var orderDetail = new OrderDetail
            {
                OrderId = createdOrder.OrderId,
                BlindBoxId = blindBox.BlindBoxId,
                Quantity = 1,
                Price = spinCost
            };

            if (!useUTC)
            {
                try
                {
                    // Convert UTC time to specified TimeZone
                    TimeZoneInfo timeZone = TZConvert.GetTimeZoneInfo(timeZoneId);
                    transactionDatetime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
                }
                catch (TimeZoneNotFoundException)
                {
                    throw new Exception("Invalid TimeZone");
                }
            }

            await _orderDetailService.AddOrderDetailAsync(orderDetail);

            await _walletTransaction.AddWalletTransactionAsync(userWallet.WalletId, spinCost, "spin", "success", transactionDatetime.ToString(dateFormat, CultureInfo.InvariantCulture), userWallet.Balance, createdOrder.OrderId);

            return Ok(new { Message = "Spin successful!", BlindBox = blindBox, Balance = userWallet.Balance });
        }
        */
        [HttpPost("spin")]
        public async Task<IActionResult> SpinWheel([FromBody] SpinRequest request)
        {
            var dateFormat = _configuration["TransactionSettings:DateFormat"] ?? "yyyy-MM-ddTHH:mm:ssZ";
            bool useUTC = bool.TryParse(_configuration["TransactionSettings:UseUTC"], out bool utc) && utc;
            var timeZoneId = _configuration["TransactionSettings:TimeZone"] ?? "UTC";
            DateTime transactionDatetime = DateTime.UtcNow; // Default to UTC

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

            // Update the spin cost for the next spin
            package.PackagePrice *= 1.1m;  // Increase price by 10% for the next spin
            var updatedPackage = new UpdatePackageRequest
            {
                CategoryId = package.CategoryId,
                PackageName = package.PackageName,
                TypeSell = package.TypeSell,
                PackagePrice = package.PackagePrice,
                Description = package.Description,
                Stock = package.Stock,
                PackageStatus = package.PackageStatus,
                Amount = package.Amount,
                DefaultPrice = package.DefaultPrice
            };
            await _packageService.UpdatePackageAsync(package.PackageId, updatedPackage);

            var blindBox = await _packageService.GetRandomBlindBoxFromPackage(request.PackageId);
            if (blindBox == null)
                return BadRequest("No BlindBox available in package.");

            if (package.Amount == 1)
            {
                // Chuyển trạng thái Package thành "SoldOut"
                await _packageService.UpdateStatusAsync(package.PackageId, "SoldOut");
            }

            await _blindBoxService.UpdateStatusAsync(blindBox.BlindBoxId, "SoldOut");

            if (package.Amount >= 1)
            {
                var newUpdatePackage = new UpdatePackageRequest
                {
                    CategoryId = package.CategoryId,
                    PackageName = package.PackageName,
                    PackagePrice = package.PackagePrice,
                    Amount = package.Amount - 1,
                    Description = package.Description,
                    Stock = package.Stock,
                    PackageStatus = package.PackageStatus,
                    TypeSell = package.TypeSell,
                };
                await _packageService.UpdatePackageAsync(request.PackageId, newUpdatePackage);
            }

            var address = await _addressService.GetDefaultAddressByAccoutId(request.AccountId);

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
                DiscountMoney = 0,
                AddressId = address.AddressId
            };

            var createdOrder = await _orderService.AddAsync(order);

            var orderDetail = new OrderDetail
            {
                OrderId = createdOrder.OrderId,
                BlindBoxId = blindBox.BlindBoxId,
                PackageId = package.PackageId,
                Quantity = 1,
                Price = spinCost
            };

            if (!useUTC)
            {
                try
                {
                    // Convert UTC time to specified TimeZone
                    TimeZoneInfo timeZone = TZConvert.GetTimeZoneInfo(timeZoneId);
                    transactionDatetime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
                }
                catch (TimeZoneNotFoundException)
                {
                    throw new Exception("Invalid TimeZone");
                }
            }

            await _orderDetailService.AddOrderDetailAsync(orderDetail);

            await _walletTransaction.AddWalletTransactionAsync(userWallet.WalletId, spinCost, "spin", "success", transactionDatetime.ToString(dateFormat, CultureInfo.InvariantCulture), userWallet.Balance, createdOrder.OrderId);

            return Ok(new { Message = "Spin successful!", BlindBox = blindBox, Balance = userWallet.Balance });
        }

    }

}

