using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanEjdg.Core.Domain.ValueTypes
{
    public class PurchasedLine
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = "";
        public decimal ProductPrice { get; set; }
        public long Quantity;

        public decimal CalculateTotalLineValue()
        {
            return ProductPrice * Quantity;
        }
    }
}
