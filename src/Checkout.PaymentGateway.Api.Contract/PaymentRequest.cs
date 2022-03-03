namespace Checkout.PaymentGateway.Api.Contract
{
    public class PaymentRequest
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public CardDetails From { get; set; }
    }
}