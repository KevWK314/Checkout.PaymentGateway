namespace Checkout.PaymentGateway.Api.Repository;

public interface IPaymentRepository
{
    Task SavePayment(Payment payment);
    Task<Payment> GetPayment(string paymentId);
}

/// <summary>
/// This repository represents the storage of payments. For this implementation everything
/// will be stored in memory. This would be replaced by a class that accesses whatever DB 
/// stores the payments.
/// </summary>
public class PaymentRepository : IPaymentRepository
{
    private readonly List<Payment> _payments = new List<Payment>();

    public Task SavePayment(Payment payment)
    {
        _payments.Add(payment);
        return Task.CompletedTask;
    }

    public Task<Payment> GetPayment(string paymentId)
    {
        return Task.FromResult(_payments.FirstOrDefault(p => p.Id == paymentId));
    }
}
