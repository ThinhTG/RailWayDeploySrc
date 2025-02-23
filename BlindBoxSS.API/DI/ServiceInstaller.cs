
using Repositories.UnitOfWork;
using Services.AccountService;
using Services.Email;
using Services;

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
        }
    }
}
