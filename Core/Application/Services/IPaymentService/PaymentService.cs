using CleanEjdg.Core.Domain.ValueTypes;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanEjdg.Core.Application.Services
{
	public class PaymentService : IPaymentService
	{
        public IImageService ImgService { get; set; }
		public PaymentService(IImageService imageService)
		{
            ImgService = imageService;
			StripeConfiguration.ApiKey = "sk_test_51NZVgyIEMenyYxYku2a919awtxfxH9hr6sX7gFkMNsOqwwrWc7LVXIPWmrQYVrZo7Bwh259DIJ2VEq6nU6SdVQa400WCPqAVpL";
		}
		public Session CreateCheckoutSession(List<CartLine> cartLines)
		{
            var lineItems = new List<SessionLineItemOptions>();
            cartLines.ForEach(cl => lineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmountDecimal = cl.Product.Price * 100,
                    Currency = "eur",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = cl.Product.Name,
                        Metadata = new Dictionary<string, string>
                        {
                            { "id" , cl.Product.Id.ToString() }
                        }                        
                        //Images = cl.Product.Photos.Select(p => ImgService.GetImageSrc(p.Bytes)).ToList()
                    }
                },
                Quantity = cl.Quantity
            }));

            var domain = "https://localhost:7137";
            var options = new SessionCreateOptions
            {
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = domain + "/order-success",
                CancelUrl = domain + "/gatos",
                PaymentMethodTypes = new List<string>
				{
                    "card"
				},
            };
            options.ShippingAddressCollection = new SessionShippingAddressCollectionOptions
            {
                AllowedCountries = new List<string> { "ES" }
            };
            var service = new SessionService();
            Session session = service.Create(options);

            return session;
        }
	}
}
