using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Email
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string message);
        Task SendConfirmationEmailAsync(ApplicationUser user, string token);
        Task ResendConfirmationEmailAsync(ApplicationUser user, string token);

        Task SendResetPasswordEmailAsync(ApplicationUser user, string token);
    }

}
