namespace Checkout.PaymentGateway.Api.Contract
{
    public class Payment
    {
        public string Id { get; set; }
        public string MerchantId { get; set; }
        public bool Success { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public CardDetails From { get; set; }
    }
}