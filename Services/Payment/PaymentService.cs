using Net.payOS.Types;
using Net.payOS;
using Services.Payment;
using Services.Request;
public class PaymentService : IPaymentService
{
    private readonly PayOS _payOS;

    public PaymentService(PayOS payOS)
    {
        _payOS = payOS;
    }

    public async Task<CreatePaymentResult> CreatePaymentLinkAsync(CreatePaymentLinkRequest request)
    {
        int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
        ItemData item = new ItemData(request.orderId, 1, request.price);
        List<ItemData> items = new List<ItemData> { item };
        PaymentData paymentData = new PaymentData(orderCode, request.price, request.description, items, request.cancelUrl, request.returnUrl);

        return await _payOS.createPaymentLink(paymentData);
    }

    public async Task<PaymentLinkInformation> GetPaymentLinkInformationAsync(int orderId)
    {
        return await _payOS.getPaymentLinkInformation(orderId);
    }

    public async Task ConfirmWebhookAsync(string webhookUrl)
    {
        await _payOS.confirmWebhook(webhookUrl);
    }

    public WebhookData VerifyPaymentWebhookData(WebhookType webhookType)
    {
        return _payOS.verifyPaymentWebhookData(webhookType);
    }
}