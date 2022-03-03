namespace Checkout.PaymentGateway.Api.Contract
{
    public class PaymentResponse
    {
        public Payment Payment { get; set; }
        public string Error { get; set; }
    }
}