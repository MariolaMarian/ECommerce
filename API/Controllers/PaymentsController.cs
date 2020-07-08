using System.IO;
using System.Threading.Tasks;
using API.Errors;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stripe;

namespace API.Controllers
{
    public class PaymentsController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<IPaymentService> _logger;
        private readonly string _whSecret;
        public PaymentsController(IPaymentService paymentService, ILogger<IPaymentService> logger, IConfiguration config)
        {
            _paymentService = paymentService;
            _logger = logger;
            _whSecret = config.GetSection("StripeSettings:WhSecret").Value;
        }

        ///<summary>
        /// To return basket with payment intent and client secret
        ///</summary>
        ///<response code="200">If payment intent was successfully created or updated</response>
        ///<response code="400">If an error occured while trying to create or update payment intent </response>
        [Produces("application/json")]
        [ProducesResponseType(typeof(CustomerBasket),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status400BadRequest)]
        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);

            if (basket == null)
            {
                return BadRequest(new ApiResponse(400, "Problem with basket"));
            }

            return Ok(basket);
        }

        ///<summary>
        /// To allow Stripe send notification to this application when payment event occurs
        ///</summary>
        ///<response code="200">Returns EmptyResult back to Stripe (Stripe would send event notifications to this application till he receives confirmation)</response>
        [HttpPost("webhook")]
        public async Task<ActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], _whSecret);

            PaymentIntent intent;
            Core.Entities.OrderAgregate.Order order;

            switch (stripeEvent.Type)
            {
                case "payment_intent.succeeded":
                    intent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("Payment succeeded: ", intent.Id);
                    order = await _paymentService.UpdateOrderPaymentSucceeded(intent.Id);
                    _logger.LogInformation("Payment received to order ", order.Id);
                    break;
                case "payment_intent.payment_failed":
                    intent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("Payment failed: ", intent.Id);
                    order = await _paymentService.UpdateOrderPaymentFailed(intent.Id);
                    _logger.LogInformation("Payment failed to order ", order.Id);
                    break;    
            }

            //striper wants confirmation, if he does not receive one, he will still send requests 
            return new EmptyResult();
        }

    }
}