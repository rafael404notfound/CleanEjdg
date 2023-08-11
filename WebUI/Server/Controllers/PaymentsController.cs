using CleanEjdg.Core.Application.Services;
using CleanEjdg.Core.Domain.ValueTypes;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using System.Net.Http.Headers;

namespace WebUI.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PaymentsController : ControllerBase
	{
		private readonly IPaymentService _paymentService;
		private readonly HttpClient _httpClient;

		public PaymentsController(IPaymentService paymentService, HttpClient httpClient)
        {
            _paymentService = paymentService;
            _httpClient = httpClient;
        }

        [HttpPost("checkout")]
		public ActionResult CreateCheckoutSession(List<CartLine> cartLines)
		{
			var session = _paymentService.CreateCheckoutSession(cartLines);
			return Ok(session.Url);
		}

        [HttpGet("sessions/{id}")]
		public async Task<ActionResult> GetSession(string id)
        {
			StripeConfiguration.ApiKey = "sk_test_51NZVgyIEMenyYxYku2a919awtxfxH9hr6sX7gFkMNsOqwwrWc7LVXIPWmrQYVrZo7Bwh259DIJ2VEq6nU6SdVQa400WCPqAVpL";

			var service = new SessionService();
			var result = service.Get(id);
			return Ok(result);
		}

		[HttpGet("sessions")]
		public async Task<ActionResult> GetSessions()
		{
			StripeConfiguration.ApiKey = "sk_test_51NZVgyIEMenyYxYku2a919awtxfxH9hr6sX7gFkMNsOqwwrWc7LVXIPWmrQYVrZo7Bwh259DIJ2VEq6nU6SdVQa400WCPqAVpL";

			var service = new SessionService();
			StripeList<Session> sessions = service.List();
			return Ok(sessions);
		}

		[HttpGet("line-items/{id}")]
		public async Task<ActionResult> GetLineItems(string id)
		{
			StripeConfiguration.ApiKey = "sk_test_51NZVgyIEMenyYxYku2a919awtxfxH9hr6sX7gFkMNsOqwwrWc7LVXIPWmrQYVrZo7Bwh259DIJ2VEq6nU6SdVQa400WCPqAVpL";

			var options = new SessionListLineItemsOptions
			{
				Limit = 5,
			};
			var service = new SessionService();
			StripeList<LineItem> lineItems = service.ListLineItems(id, options);
			return Ok(lineItems);
		}
	}
}
