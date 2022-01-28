namespace Checkout.PaymentGateway.Api.Contract;

public record Payment
{
    public string Id { get; init; }
    public string MerchantId { get; init; }
    public bool Success { get; init; }
    public decimal Amount { get; init; }
    public string Currency { get; init; }
    public CardDetails From { get; init; }
}
