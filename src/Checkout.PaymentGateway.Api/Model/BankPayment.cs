using Checkout.PaymentGateway.Api.Contract;

namespace Checkout.PaymentGateway.Api.Model
{
    public class BankPayment
    {
        public bool Success { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string BankAccount { get; set; }
        public CardDetails From { get; set; }
    }
}