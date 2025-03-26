namespace Services.Request
{
    public class CreatePaymentLinkRequest
    {
        public int orderId { get; set; }
        public string description = "Payment ";
        public int price { get; set; }
        public string returnUrl = "https://mystic-blind-box.web.app/payment-success";
        public string cancelUrl = "https://mystic-blind-box.web.app/payment-fail";
    }
}