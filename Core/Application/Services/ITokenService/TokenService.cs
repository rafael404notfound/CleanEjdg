﻿using CleanEjdg.Core.Domain.ValueTypes;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace CleanEjdg.Core.Application.Services
{
    public class TokenService : ITokenService
    {
        public IJSRuntime JSRuntime { get; set; }
        public HttpClient HttpClient { get; set; }
        
        public TokenService (IJSRuntime jSRuntime, HttpClient httpClient)
        {
            JSRuntime = jSRuntime;
            HttpClient = httpClient;
        }

        public async Task<LoginResult> CreateToken(UserCredentials user)
        {
            var response = await HttpClient.PostAsJsonAsync<UserCredentials>("api/Auth/login", user);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<LoginResult>() ?? new LoginResult();
            }
            return new LoginResult();
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
                if (dataArray.Length == 3)
                {
                    token = dataArray[2];
                }
            }
            return token;
        }

        public async Task SetToken(LoginResult token)
        {
            await JSRuntime.InvokeVoidAsync("localStorage.setItem", "user", $"{token.UserName};{token.Email};{token.JwtBearer}").ConfigureAwait(false);
        }

        public async Task<string> GetUsername()
        {
            var user = await JSRuntime.InvokeAsync<string>("localStorage.getItem", "user").ConfigureAwait(false);
            if (!string.IsNullOrWhiteSpace(user))
            {
                var dataArray = user.Split(';', 3);
                if (dataArray.Length == 3)
                {
                    user = dataArray[0];
                }
            }
            return user;
        }
    }
}
