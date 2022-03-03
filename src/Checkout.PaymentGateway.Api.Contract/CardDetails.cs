namespace Checkout.PaymentGateway.Api.Contract
{
    public class CardDetails
    {
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public string Cvv { get; set; }
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
    }
}