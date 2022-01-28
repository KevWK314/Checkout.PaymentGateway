using Microsoft.AspNetCore.Mvc;

namespace Checkout.PaymentGateway.Api.Controllers
{
    [ApiController]
    [Route("api/payment")]
    public class PaymentController : Controller
    {
        private readonly ICreatePaymentHandler _createPaymentHandler;
        private readonly IGetPaymentHandler _getPaymentHandler;

        public PaymentController(ICreatePaymentHandler createPaymentHandler, IGetPaymentHandler getPaymentHandler)
        {
            _createPaymentHandler = createPaymentHandler;
            _getPaymentHandler = getPaymentHandler;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePayment([FromQuery] string merchantkey, PaymentRequest request)
        {
            var response = await _createPaymentHandler.Process(merchantkey, request);

            // This is very un-nuanced in it's response. I'm assuming if there is
            // and error it's because the request is invalid in some way. There would 
            // need to be thought into exactly what response is expected in specific
            // circumstances (invalid merchant, error calling bank, etc).
            if (!string.IsNullOrEmpty(response.Error))
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("{paymentId}")]
        public async Task<IActionResult> GetPayment([FromQuery] string merchantkey, string paymentId)
        {
            var response = await _getPaymentHandler.Process(merchantkey, paymentId);

            // Same as above
            if (!string.IsNullOrEmpty(response.Error))
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
