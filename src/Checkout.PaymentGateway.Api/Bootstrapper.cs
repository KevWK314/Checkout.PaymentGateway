namespace Checkout.PaymentGateway.Api;

public static class Bootstrapper
{
    /// <summary>
    /// Register all dependencies in this Bootstrapper. I quite like simply
    /// using IServiceCollection, especially when the DI requirements are simple.
    /// </summary>
    public static void Bootstrap(IServiceCollection services)
    {
        services.AddSingleton<ICreatePaymentHandler, CreatePaymentHandler>();
        services.AddSingleton<IGetPaymentHandler, GetPaymentHandler>();

        services.AddSingleton<IBankPaymentMapper, BankPaymentMapper>();

        services.AddSingleton<IMerchantClient, MerchantClient>();
        services.AddSingleton<IBankClient, BankClient>();
        
        services.AddSingleton<IPaymentRepository, PaymentRepository>();
    }
}
