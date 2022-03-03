using Checkout.PaymentGateway.Api.Contract;
using Checkout.PaymentGateway.Api.Model;
using System.Threading.Tasks;

namespace Checkout.PaymentGateway.Api.Client
{
    public interface IBankClient
    {
        Task<BankPayment> ProcessPayment(Merchant merchant, PaymentRequest paymentRequest);
    }

    public class BankClient : IBankClient
    {
        private const string InvalidMerchantAccount = "Invalid";

        public Task<BankPayment> ProcessPayment(Merchant merchant, PaymentRequest paymentRequest)
        {
            // This implementation will allow for the mocked response from an Acquiring Bank.
            // I will mock 2 possibile responses - sucess or failed. Not fully understanding all the
            // failure scenarios, I'm opting to keep it simple.

            // The mocking logic will work hand in hand with the Merchant client where I've configured
            // merchants one of which will always fail in this process.

            var isSuccess = merchant.BankAccount != InvalidMerchantAccount;

            return Task.FromResult(new BankPayment
            {
                Amount = paymentRequest.Amount,
                Currency = paymentRequest.Currency,
                BankAccount = merchant.BankAccount,
                From = paymentRequest.From,
                Success = isSuccess
            });
        }
    }
}