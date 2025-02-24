using BlindBoxSS.API.DI;
using BlindBoxSS.API;
using BlindBoxSS.API.Exceptions;
using BlindBoxSS.API.Extensions;
using BlindBoxSS.API;
using DAO.Mapping;
using DAO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Models;
using Net.payOS;
using Services.AccountService;
using Services.Email;
using Services;
using Repositories.WalletRepo;
using BlindBoxSS.API.Services;
using Repositories.Product;
using Services.Product;
using Services.Wallet;
using Services.Payment;
using System.Runtime.ConstrainedExecution;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

// Đăng ký dịch vụ thông qua DI Installer
builder.Services.InstallerServicesInAssembly(builder.Configuration);

var app = builder.Build();

app.UseCors("AllowAll");

// Chạy SeedRoles để tạo tài khoản mặc định nếu chưa có
var scope = app.Services.CreateScope();
await SeedRoles.InitializeRoles(scope.ServiceProvider);

// Cấu hình Swagger cho môi trường Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Handle 403 errors
app.Use(async (context, next) =>
{
    await next();
    if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
    {
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync("{ \"message\": \"You don't have permission for this action. Please login with an Admin account.\" }");
    }
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();