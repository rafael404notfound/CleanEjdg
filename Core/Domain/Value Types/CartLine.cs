using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanEjdg.Core.Domain.Entities;

namespace CleanEjdg.Core.Domain.ValueTypes
{
    public class CartLine
    {
        public int Id { get; set; }
        public Product Product { get; set; } = new Product();
        public int Quantity { get; set; }
    }
}
