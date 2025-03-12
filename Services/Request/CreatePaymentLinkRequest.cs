namespace Services.Request
{
    public record CreatePaymentLinkRequest(
        string orderId,
        string description,
        int price,
        string returnUrl,
        string cancelUrl
    );
}