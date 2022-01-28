namespace Checkout.PaymentGateway.Api.Model;

public record BankPayment
{
    public bool Success { get; init; }
    public decimal Amount { get; init; }
    public string Currency { get; init; }
    public string BankAccount { get; init; }
    public CardDetails From { get; init; }
}
