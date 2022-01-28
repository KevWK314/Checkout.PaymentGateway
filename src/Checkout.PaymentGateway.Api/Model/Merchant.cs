namespace Checkout.PaymentGateway.Api.Model;

public record Merchant
{
    public string Id { get; init; }
    public string Name { get; init; }
    public string BankAccount { get; init; }
    public bool IsActive { get; init; }
}
