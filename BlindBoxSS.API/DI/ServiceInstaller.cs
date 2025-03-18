using Repositories.UnitOfWork;
using Services.AccountService;
using Services.Email;
using Services;
using BlindBoxSS.API.Services;
using Repositories.Product;
using Repositories.WalletRepo;
using Services.Payment;
using Services.Product;
using Services.Wallet;
using Services.OrderS;
using Repositories.OrderRep;
using Repositories.CategoryRepo;
using Services.CategoryS;
using Repositories.VocherRepo;
using Services.VocherS;
using Repositories.AddressRepo;
using Services.AddressS;

namespace BlindBoxSS.API.DI
{
    public class ServiceInstaller : IInstaller
    {
        public void InstallService(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ICartService, CartService>();
           services.AddScoped<IPaymentService, PaymentService>();
           services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<IWalletTransactionRepository, WalletTransactionRepository>();
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<IWalletTransactionService, WalletTransactionService>();
            services.AddScoped<IBlindBoxRepository, BlindBoxRepository>();
            services.AddScoped<IBlindBoxService, BlindBoxService>();
            services.AddScoped<IPackageRepository, PackageRepository>();
            services.AddScoped<IPackageService, PackageService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderDetailService, OrderDetailService>();
            services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IVocherRepository, VocherRepository>();
            services.AddScoped<IVocherService, VocherService>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IAddressService, AddressService>();
        }
    }
}
