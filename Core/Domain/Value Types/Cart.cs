using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanEjdg.Core.Domain.Entities;

namespace CleanEjdg.Core.Domain.ValueTypes
{
    public class Cart
    {
        public List<CartLine> Lines { get; set; } = new List<CartLine>();

        public void AddItem(Product product, int quantity)
        {
            CartLine? line = Lines.Where(l => l.Product.Id == product.Id).FirstOrDefault();

            if(line == null)
            {
                Lines.Add(new CartLine { Product = product, Quantity = quantity });
            } else
            {
                line.Quantity += quantity;
            }
        }

        public void RemoveLine(Product product) => Lines.RemoveAll(l => l.Product.Id == product.Id);

        public decimal ComputeTotalValue() => Lines.Sum(l => l.Product.Price * l.Quantity);

        public void Clear() => Lines.Clear();
    }
}
