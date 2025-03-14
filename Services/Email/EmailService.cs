using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using Models;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Services.Email
{


    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
       

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentNullException(nameof(email), "Recipient email cannot be null or empty");
            }

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Blind Box Sales Website", _configuration["EmailSettings:SenderEmail"]));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("html") { Text = message };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_configuration["EmailSettings:SmtpServer"],
                                          int.Parse(_configuration["EmailSettings:Port"]),
                                          MailKit.Security.SecureSocketOptions.StartTls);

                await client.AuthenticateAsync(_configuration["EmailSettings:SenderEmail"],
                                               _configuration["EmailSettings:SenderPassword"]);

                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }


        public async Task SendConfirmationEmailAsync(ApplicationUser user, string token)
        {

            var encodedToken = Uri.EscapeDataString(token);
            var confirmationLink = $"http://localhost:5000/api/confirm-email?userId={user.Id}&token={encodedToken}";
            var message = $"Vui lòng nhấp vào link để xác thực email: <a href='{confirmationLink}'>Xác nhận</a>";
            await SendEmailAsync(user.Email, "Xác thực email", message);
        }

        public async Task ResendConfirmationEmailAsync(ApplicationUser user, string token)
        {
            var encodedToken = Uri.EscapeDataString(token);
            var confirmationLink = $"http://localhost:5000/api/confirm-email?userId={user.Id}&token={encodedToken}";
            var message = $"Bạn đã yêu cầu xác thực lại email. Nhấp vào link sau để xác nhận: <a href='{confirmationLink}'>Xác nhận</a>";

            await SendEmailAsync(user.Email, "Xác thực lại email", message);
        }

        public async Task SendResetPasswordEmailAsync(ApplicationUser user, string token)
        {
            var encodedToken = Uri.EscapeDataString(token);
            var resetLink = $"http://localhost:3000/reset-password?email={user.Email}&token={encodedToken}";
            var message = $"Nhấp vào link để đặt lại mật khẩu: <a href='{resetLink}'>Đặt lại mật khẩu</a>";

            await SendEmailAsync(user.Email, "Đặt lại mật khẩu", message);
        }



    }




}

