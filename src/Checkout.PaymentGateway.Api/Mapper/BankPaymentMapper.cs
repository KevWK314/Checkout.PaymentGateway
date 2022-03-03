using Checkout.PaymentGateway.Api.Contract;
using Checkout.PaymentGateway.Api.Model;
using System;

namespace Checkout.PaymentGateway.Api.Mapper
{
    public interface IBankPaymentMapper
    {
        Payment Map(string merchantId, BankPayment bankPayment);
    }

    /// <summary>
    /// Map a BankPayment to a Payment type that we can use in our service
    /// </summary>
    public class BankPaymentMapper : IBankPaymentMapper
    {
        public Payment Map(string merchantId, BankPayment bankPayment)
        {
            return new Payment
            {
                Id = Guid.NewGuid().ToString(),
                MerchantId = merchantId,
                Amount = bankPayment.Amount,
                Currency = bankPayment.Currency,
                From = bankPayment.From,
                Success = bankPayment.Success
            };
        }
    }
}