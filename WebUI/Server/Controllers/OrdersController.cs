using Microsoft.AspNetCore.Mvc;
using CleanEjdg.Core.Application.Services;
using CleanEjdg.Core.Domain.Entities;
using CleanEjdg.Core.Application.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Stripe;
using Stripe.Checkout;
using CleanEjdg.Core.Domain.ValueTypes;

namespace WebUI.Server.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        IRepositoryBase<Order> OrderRepo;
        IRepositoryBase<CleanEjdg.Core.Domain.Entities.Product> ProductRepo;
        HttpClient _httpClient;

        public OrdersController(IRepositoryBase<Order> orderRepo, HttpClient httpClient, IRepositoryBase<CleanEjdg.Core.Domain.Entities.Product> productRepo)
        {
            OrderRepo = orderRepo;
            ProductRepo = productRepo;
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            IEnumerable<Order> savedOrders = OrderRepo.GetAll();
            IEnumerable<CleanEjdg.Core.Domain.Entities.Product> products = ProductRepo.GetAll();

            StripeConfiguration.ApiKey = "sk_test_51NZVgyIEMenyYxYku2a919awtxfxH9hr6sX7gFkMNsOqwwrWc7LVXIPWmrQYVrZo7Bwh259DIJ2VEq6nU6SdVQa400WCPqAVpL";

            var service = new SessionService();
            StripeList<Session> sessions = service.List();

            if(sessions != null)
            {
                foreach(var session in sessions)
                {
                    if (savedOrders.FirstOrDefault(so=> so.StripeSessionId == session.Id) == null)
                    {
                        var lineItems = service.ListLineItems(session.Id);
                        var purchasedLines = new List<PurchasedLine>();
                        foreach(var lineItem in lineItems)
                        {
                            purchasedLines.Add(new PurchasedLine
                            {
                                ProductName = lineItem.Description,
                                ProductPrice = (decimal)lineItem.Price.UnitAmountDecimal,
                                Quantity = (long)lineItem.Quantity,
                                ProductId = products.FirstOrDefault(p => p.Name == lineItem.Description).Id
                            }) ;
                        }
                        var newOrder = new Order
                        {
                            StripeSessionId = session.Id,
                            CreatedAt = session.Created,
                            SuccessfulPayment = session.PaymentStatus == "paid" ? true : false,
                            TotalPaymet = (decimal)(session.AmountTotal == null ? 0 : session.AmountTotal / 100),
                            ShippingAddress = new CleanEjdg.Core.Domain.ValueTypes.Address
                            {
                                City = session.CustomerDetails?.Address.City ?? "null",
                                Country = session.CustomerDetails?.Address.Country ?? "null",
                                Line1 = session.CustomerDetails?.Address.Line1 ?? "null",
                                Line2 = session.CustomerDetails?.Address.Line2 ?? "null",
                                PostalCode = session.CustomerDetails?.Address.PostalCode ?? "null",
                            },
                            PurchasedLines = purchasedLines,
                            CustomerEmail = session.CustomerDetails?.Email ?? "null",
                        };
                        OrderRepo.Create(newOrder);
                    }
                }
            }       
            IEnumerable<Order> orders = OrderRepo.GetAll();
            if (orders.Count() != 0 && orders != null)
            {
                return Ok(orders);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            await OrderRepo.Delete(id);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrder([FromBody]Order order)
        {
            await OrderRepo.Update(order);
            return Ok();
        }
    }
}