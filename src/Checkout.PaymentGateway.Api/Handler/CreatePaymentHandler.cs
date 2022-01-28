namespace Checkout.PaymentGateway.Api.Handler;

public interface ICreatePaymentHandler
{
    Task<PaymentResponse> Process(string merchantSecret, PaymentRequest paymentRequest);
}

/// <summary>
/// Process a create payment request and return a success or error response.
/// </summary>
public class CreatePaymentHandler : ICreatePaymentHandler
{
    private readonly ILogger<CreatePaymentHandler> _logger;
    private readonly IMerchantClient _merchantClient;
    private readonly IBankClient _bankClient;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IBankPaymentMapper _bankPaymentMapper;

    public CreatePaymentHandler(
        ILogger<CreatePaymentHandler> logger,
        IMerchantClient merchantClient,
        IBankClient bankClient,
        IPaymentRepository paymentRepository,
        IBankPaymentMapper bankPaymentMapper)
    {
        _logger = logger;
        _merchantClient = merchantClient;
        _bankClient = bankClient;
        _paymentRepository = paymentRepository;
        _bankPaymentMapper = bankPaymentMapper;
    }

    public async Task<PaymentResponse> Process(string merchantSecret, PaymentRequest paymentRequest)
    {
        try
        {
            var merchant = await _merchantClient.GetMerchant(merchantSecret);
            if(merchant?.IsActive == false)
                return new PaymentResponse { Error = "Invalid merchant." };

            var bankPayment = await _bankClient.ProcessPayment(merchant, paymentRequest);

            var payment = _bankPaymentMapper.Map(merchant.Id, bankPayment);

            await _paymentRepository.SavePayment(payment);

            return new PaymentResponse { Payment = payment };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process payment");
            return new PaymentResponse { Error = "Failed to process the payment. Please try again later." };
        }
    }
}
