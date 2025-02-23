using BlindBoxSS.API.DI;
using BlindBoxSS.API;

var builder = WebApplication.CreateBuilder(args);

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

// Xử lý lỗi 403
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