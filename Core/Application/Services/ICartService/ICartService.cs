using CleanEjdg.Core.Domain.ValueTypes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanEjdg.Core.Application.Services
{
    public interface ICartService
    {
        Cart Cart { get; set; }
        Task GetCart();
        Task AddItem(Product p, int q);
        Task RemoveLine(Product p);
        Task Clear();
        Task<string> Checkout();
    }
}
