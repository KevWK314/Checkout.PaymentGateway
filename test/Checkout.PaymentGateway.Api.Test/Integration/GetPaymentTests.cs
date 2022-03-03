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
    public class GetPaymentTests : VerifyBase
    {
        private readonly HttpClient _paymentGatewayClient;

        public GetPaymentTests()
            : base()
        {
            _paymentGatewayClient = PaymentGatewayClient.Create();
        }

        [Fact]
        public async Task WhenInvalidMerchantKey_ShouldFail()
        {
            var response = await _paymentGatewayClient.GetAsync("api/payment/1234?merchantkey=invalidkey");
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var paymentResponse = await response.Content.ReadFromJsonAsync<PaymentResponse>();
            await Verify(paymentResponse);
        }

        [Fact]
        public async Task WhenInvalidPaymentId_ShouldReturnError()
        {
            var getResponse = await _paymentGatewayClient.GetAsync($"api/payment/invalidId?merchantkey=merchantkey1");
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var getPaymentResponse = await getResponse.Content.ReadFromJsonAsync<PaymentResponse>();
            await Verify(getPaymentResponse);
        }

        [Fact]
        public async Task WhenValidPaymentId_ShouldReturnPayment()
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
            var createResponse = await _paymentGatewayClient.PostAsJsonAsync("api/payment?merchantkey=merchantkey1", request);
            var createPaymentResponse = await createResponse.Content.ReadFromJsonAsync<PaymentResponse>();

            var getResponse = await _paymentGatewayClient.GetAsync($"api/payment/{createPaymentResponse.Payment.Id}?merchantkey=merchantkey1");
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var getPaymentResponse = await getResponse.Content.ReadFromJsonAsync<PaymentResponse>();
            await Verify(getPaymentResponse);
        }

        [Fact]
        public async Task WhenValidPaymentIdButIncorrectMerchantKey_ShouldReturnError()
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
            var createResponse = await _paymentGatewayClient.PostAsJsonAsync("api/payment?merchantkey=merchantkey1", request);
            var createPaymentResponse = await createResponse.Content.ReadFromJsonAsync<PaymentResponse>();

            var getResponse = await _paymentGatewayClient.GetAsync($"api/payment/{createPaymentResponse.Payment.Id}?merchantkey=merchantkey2");
            getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var getPaymentResponse = await getResponse.Content.ReadFromJsonAsync<PaymentResponse>();
            await Verify(getPaymentResponse);
        }
    }
}