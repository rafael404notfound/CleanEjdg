using CleanEjdg.Core.Application;
using CleanEjdg.Core.Domain.ValueTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CleanEjdg.Core.Application.Services
{
    public  class LocalStorageCartService : ICartService
    {
        public Cart Cart { get; set; } = new Cart();
        public IJSRuntime JSRuntime { get; set; }
        private readonly HttpClient HttpClient;

        public LocalStorageCartService(IJSRuntime jSRuntime, HttpClient httpClient)
        {
            JSRuntime = jSRuntime;
            HttpClient = httpClient;
        }

        public async Task AddItem(Product p, int q)
        {
            Cart.AddItem(p, q);
            var cartJson = JsonSerializer.Serialize(Cart);
            await JSRuntime.InvokeVoidAsync("localStorage.setItem", "cart", $"{cartJson}").ConfigureAwait(false);

        }

        public async Task Clear()
        {
            Cart.Clear();
            await JSRuntime.InvokeAsync<string>("localStorage.removeItem", "cart").ConfigureAwait(false);
        }

        public async Task GetCart()
        {
            var cartJson = await JSRuntime.InvokeAsync<string>("localStorage.getItem", "cart").ConfigureAwait(false);
            if (!string.IsNullOrWhiteSpace(cartJson)) Cart = JsonSerializer.Deserialize<Cart>(cartJson);            
        }

        public async Task RemoveLine(Product p)
        {
            Cart.RemoveLine(p);
            var cartJson = JsonSerializer.Serialize(Cart);
            await JSRuntime.InvokeVoidAsync("localStorage.setItem", "cart", $"{cartJson}").ConfigureAwait(false);
        }

		public async Task<string> Checkout()
		{
            var result = await HttpClient.PostAsJsonAsync("api/payments/checkout", Cart.Lines);
            var url = await result.Content.ReadAsStringAsync();
            return url;
		}
	}
}
