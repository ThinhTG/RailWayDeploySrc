namespace Services.Request
{
    public class CreatePaymentLinkRequestV2
    {
        public string? accountId { get; set; }
        public string description = "Deposit ";
        public int price { get; set; }
        public string returnUrl = "https://mystic-blind-box.web.app/wallet-success";
        public string cancelUrl = "https://mystic-blind-box.web.app/wallet-fail";
    }
}