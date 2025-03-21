namespace Services.Request
{
    public class CreatePaymentLinkRequest
    {
        public int orderId { get; set; }
        public string description = "Payment ";
        public int price { get; set; }
        public string returnUrl = "http://localhost:3000/payment-success";
        public string cancelUrl = "http://localhost:3000/payment-fail";
    }
}