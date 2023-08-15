using CleanEjdg.Core.Domain.ValueTypes;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CleanEjdg.Core.Application.Services
{
    public class TokenService : ITokenService
    {
        public IJSRuntime JSRuntime { get; set; }
        
        public TokenService (IJSRuntime jSRuntime, HttpClient httpClient)
        {
            JSRuntime = jSRuntime;
        }

        public async Task DeleteToken()
        {
            await JSRuntime.InvokeAsync<string>("localStorage.removeItem", "user").ConfigureAwait(false);
        }

        public async Task<string> GetToken()
        {
            var token = await JSRuntime.InvokeAsync<string>("localStorage.getItem", "user").ConfigureAwait(false);
            if (!string.IsNullOrWhiteSpace(token))
            {
                var dataArray = token.Split(';');
                if (dataArray.Length == 4)
                {
                    token = dataArray[2];
                }
            }
            return token;
        }
        public async Task<string> GetRefreshToken()
        {
            var token = await JSRuntime.InvokeAsync<string>("localStorage.getItem", "user").ConfigureAwait(false);
            if (!string.IsNullOrWhiteSpace(token))
            {
                var dataArray = token.Split(';');
                if (dataArray.Length == 4)
                {
                    token = dataArray[3];
                }
            }
            return token;
        }

        public async Task SetToken(Token token)
        {
            await JSRuntime.InvokeVoidAsync("localStorage.setItem", "user", $"SomeUserName;SomeEmail;{token.AccessToken};{token.RefreshToken}").ConfigureAwait(false);
        }

        public async Task<string> GetUsername()
        {
            var user = await JSRuntime.InvokeAsync<string>("localStorage.getItem", "user").ConfigureAwait(false);
            if (!string.IsNullOrWhiteSpace(user))
            {
                var dataArray = user.Split(';', 4);
                if (dataArray.Length == 4)
                {
                    user = dataArray[0];
                }
            }
            return user;
        }
    }
}
