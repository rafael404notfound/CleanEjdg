using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanEjdg.Core.Domain.ValueTypes
{
    public class Address
    {
        public string City { get; set; } = "";
        public string Country { get; set; } = "";
        public string Line1 { get; set; } = "";
        public string Line2 { get; set; } = "";
        public string PostalCode { get; set; } = "";
    }
}
