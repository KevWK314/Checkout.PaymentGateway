namespace Checkout.PaymentGateway.Api.Contract;

public record CardDetails
{
    public string CardHolderName { get; init; }
    public string CardNumber { get; init; }
    public string Cvv { get; init; }
    public int ExpiryMonth { get; init; }
    public int ExpiryYear { get; init; }
}