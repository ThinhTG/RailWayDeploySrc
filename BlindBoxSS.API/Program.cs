using BlindBoxSS.API;
using BlindBoxSS.API.DI;
using Net.payOS;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();
PayOS payOS = new PayOS(configuration["PaymentEnvironment:PAYOS_CLIENT_ID"] ?? throw new Exception("Cannot find payment environment"),
                    configuration["PaymentEnvironment:PAYOS_API_KEY"] ?? throw new Exception("Cannot find payment environment"),
                    configuration["PaymentEnvironment:PAYOS_CHECKSUM_KEY"] ?? throw new Exception("Cannot find payment environment"));
builder.Services.AddSingleton(payOS);

// Đăng ký dịch vụ thông qua DI Installer
builder.Services.InstallerServicesInAssembly(builder.Configuration);

var app = builder.Build();
// Cấu hình CORS
app.Use(async (context, next) =>
{
    context.Response.Headers["Cross-Origin-Opener-Policy"] = "same-origin-allow-popups";
    await next();
});


app.UseCors("AllowAll");

// Chạy SeedRoles để tạo tài khoản mặc định nếu chưa có
var scope = app.Services.CreateScope();
await SeedRoles.InitializeRoles(scope.ServiceProvider);


    app.UseSwagger();
    app.UseSwaggerUI();


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