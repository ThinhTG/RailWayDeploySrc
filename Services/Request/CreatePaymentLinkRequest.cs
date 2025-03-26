namespace Services.Request
{
    public class CreatePaymentLinkRequest
    {
        public int orderId { get; set; }
        public string description = "Payment ";
        public int price { get; set; }
        public string returnUrl = "https://railwaydeploysrc-production.up.railway.app/payment-success";
        public string cancelUrl = "https://railwaydeploysrc-production.up.railway.app/payment-fail";
    }
}