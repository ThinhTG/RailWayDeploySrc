namespace Services.Request
{
    public class CreatePaymentLinkRequestV2
    {
        public string? accountId { get; set; }
        public string description = "Deposit ";
        public int price { get; set; }
        public string returnUrl = "http://localhost:3000/payment-success";
        public string cancelUrl = "http://localhost:3000/payment-fail";
    }
}