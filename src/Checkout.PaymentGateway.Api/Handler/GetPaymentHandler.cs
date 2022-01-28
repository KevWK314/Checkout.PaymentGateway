namespace Checkout.PaymentGateway.Api.Handler;

public interface IGetPaymentHandler
{
    Task<PaymentResponse> Process(string merchantSecret, string paymentId);
}

/// <summary>
/// Get Payment based on a request payment Id. Be sure we don't return
/// payments for the incorrect merchant.
/// </summary>
public class GetPaymentHandler : IGetPaymentHandler
{
    private readonly ILogger<GetPaymentHandler> _logger;
    private readonly IMerchantClient _merchantClient;
    private readonly IPaymentRepository _paymentRepository;

    public GetPaymentHandler(
        ILogger<GetPaymentHandler> logger,
        IMerchantClient merchantClient,
        IPaymentRepository paymentRepository)
    {
        _logger = logger;
        _merchantClient = merchantClient;
        _paymentRepository = paymentRepository;
    }

    public async Task<PaymentResponse> Process(string merchantSecret, string paymentId)
    {
        try
        {
            var merchant = await _merchantClient.GetMerchant(merchantSecret);
            if (merchant == null || merchant.IsActive == false)
                return new PaymentResponse { Error = "Invalid merchant." };

            var payment = await _paymentRepository.GetPayment(paymentId);

            // If payment not found or pament belongs to another merchant, return error
            if (payment == null || payment.MerchantId != merchant.Id)
            {
                return new PaymentResponse { Error = "Payment was not found." };
            }

            return new PaymentResponse { Payment = payment };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get payment");
            return new PaymentResponse { Error = "Failed to get the payment. Please try again later." };
        }
    }
}
