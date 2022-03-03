namespace Checkout.PaymentGateway.Api.Model
{
    public class Merchant
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string BankAccount { get; set; }
        public bool IsActive { get; set; }
    }
}