using CleanEjdg.Core.Domain.ValueTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanEjdg.Core.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public List<PurchasedLine> PurchasedLines { get; set; } = new List<PurchasedLine>();
        public decimal TotalPaymet { get; set; }
        public Address ShippingAddress { get; set; } = new Address();
        public DateTime CreatedAt {  get; set; }
        public bool SuccessfulPayment { get; set; }
        public string StripeSessionId { get; set; } = String.Empty;
        public string CustomerEmail { get; set; } = String.Empty;
        public bool Sent { get; set; }
    }
}
