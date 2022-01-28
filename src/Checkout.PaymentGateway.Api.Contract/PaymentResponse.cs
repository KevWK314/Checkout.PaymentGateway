namespace Checkout.PaymentGateway.Api.Contract;

public record PaymentResponse
{
    public Payment Payment { get; init; }
    public string Error { get; init; }
}