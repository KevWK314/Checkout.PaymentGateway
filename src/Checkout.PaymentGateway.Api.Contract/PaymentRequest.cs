namespace Checkout.PaymentGateway.Api.Contract;

public record PaymentRequest
{
    public decimal Amount { get; init; }
    public string Currency { get; init; }
    public CardDetails From { get; init; }
}
