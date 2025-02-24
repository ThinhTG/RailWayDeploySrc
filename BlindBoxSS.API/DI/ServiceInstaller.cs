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
        }
    }
}
