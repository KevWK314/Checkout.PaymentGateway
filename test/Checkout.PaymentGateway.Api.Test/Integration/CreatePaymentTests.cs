using Checkout.PaymentGateway.Api.Contract;
using FluentAssertions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VerifyXunit;
using Xunit;

namespace Checkout.PaymentGateway.Api.Test.Integration
{
    [UsesVerify]
    public class CreatePaymentTests : VerifyBase
    {
        private readonly HttpClient _paymentGatewayClient;

        public CreatePaymentTests()
            : base()
        {
            _paymentGatewayClient = PaymentGatewayClient.Create();
        }

        [Fact]
        public async Task WhenInvalidMerchantKey_ShouldFail()
        {
            var request = new PaymentRequest
            {
                Amount = 24.5m,
                Currency = "GBP",
                From = new CardDetails
                {
                    CardNumber = "1234123412341234",
                    CardHolderName = "A Individual",
                    Cvv = "123",
                    ExpiryMonth = 12,
                    ExpiryYear = 22
                }
            };
            var response = await _paymentGatewayClient.PostAsJsonAsync("api/payment?merchantkey=invalidkey", request);
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var paymentResponse = await response.Content.ReadFromJsonAsync<PaymentResponse>();
            await Verify(paymentResponse);
        }

        [Fact]
        public async Task WhenValidMerchantAndRequest_ShouldReturnPaymentAsSuccess()
        {
            var request = new PaymentRequest
            {
                Amount = 24.5m,
                Currency = "GBP",
                From = new CardDetails
                {
                    CardNumber = "1234123412341234",
                    CardHolderName = "A Individual",
                    Cvv = "123",
                    ExpiryMonth = 12,
                    ExpiryYear = 22
                }
            };
            var response = await _paymentGatewayClient.PostAsJsonAsync("api/payment?merchantkey=merchantkey1", request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var paymentResponse = await response.Content.ReadFromJsonAsync<PaymentResponse>();
            await Verify(paymentResponse);
        }

        [Fact]
        public async Task WhenValidMerchantAndRequest_ShouldReturnPaymentAsNotSuccess()
        {
            var request = new PaymentRequest
            {
                Amount = 24.5m,
                Currency = "GBP",
                From = new CardDetails
                {
                    CardNumber = "1234123412341234",
                    CardHolderName = "A Individual",
                    Cvv = "123",
                    ExpiryMonth = 12,
                    ExpiryYear = 22
                }
            };
            var response = await _paymentGatewayClient.PostAsJsonAsync("api/payment?merchantkey=merchantkey4", request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var paymentResponse = await response.Content.ReadFromJsonAsync<PaymentResponse>();
            await Verify(paymentResponse);
        }
    }
}