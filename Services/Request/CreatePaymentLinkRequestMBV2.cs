namespace Services.Request
{
    public class CreatePaymentLinkRequestMBV2
    {
        public string? accountId { get; set; }
        public string description = "Deposit ";
        public int price { get; set; }
        public string returnUrl = "blindboxmobile://payment-success";
        public string cancelUrl = "blindboxmobile://payment-failed";
    }
}