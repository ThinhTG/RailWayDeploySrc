using Net.payOS.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Request;

namespace Services.Payment
{
    public interface IPaymentService
    {
        Task<CreatePaymentResult> CreatePaymentLinkAsync(CreatePaymentLinkRequest request);
        Task<CreatePaymentResult> CreatePaymentLinkDepositAsync(CreatePaymentLinkRequestV2 request);
        Task<CreatePaymentResult> CreatePaymentLinkMBAsync(CreatePaymentLinkRequestMB request);
        Task<CreatePaymentResult> CreatePaymentLinkDepositMBAsync(CreatePaymentLinkRequestMBV2 request);
        Task<PaymentLinkInformation> GetPaymentLinkInformationAsync(int orderCode);
        Task ConfirmWebhookAsync(string webhookUrl);
        WebhookData VerifyPaymentWebhookData(WebhookType webhookType);
    }
}
