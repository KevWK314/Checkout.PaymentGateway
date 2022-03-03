using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http;

namespace Checkout.PaymentGateway.Api.Test.Integration
{
    internal static class PaymentGatewayClient
    {
        /// <summary>
        /// This will create an HttpClient that can be used to send
        /// request to our service. Allows us to run integration tests on our service.
        /// Ordinarily you would want to inject mocked versions of endpoints to 
        /// ensure you don't have dependencies, but given everything is mocked in this
        /// service we don't have to worry.
        /// </summary>
        public static HttpClient Create()
        {
            var application = new WebApplicationFactory<Program>();
            return application.CreateClient();
        }
    }
}