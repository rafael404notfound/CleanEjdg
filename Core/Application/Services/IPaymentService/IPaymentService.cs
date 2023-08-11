using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanEjdg.Core.Domain.ValueTypes;
using Stripe.Checkout;

namespace CleanEjdg.Core.Application.Services
{
	public interface IPaymentService
	{
		Session CreateCheckoutSession(List<CartLine> cartLines);
	}
}
