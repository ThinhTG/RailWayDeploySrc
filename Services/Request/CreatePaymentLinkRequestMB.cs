namespace Services.Request
{
    public class CreatePaymentLinkRequestMB
    {
        public int orderId { get; set; }
        public string description = "Payment ";
        public int price { get; set; }
        public string returnUrl = "blindboxmobile://payment-success";
        public string cancelUrl = "blindboxmobile://payment-failed";
    }
}