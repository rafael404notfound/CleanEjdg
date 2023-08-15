using CleanEjdg.Core.Domain.ValueTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace CleanEjdg.Core.Application.Services
{
    public class JwtHttpClient
    {
        private readonly ITokenService _tokenService;
        private readonly HttpClient _httpClient;

        public JwtHttpClient(ITokenService tokenService, HttpClient httpClient)
        {
            _tokenService = tokenService;
            _httpClient = httpClient;
        }
        public async Task<HttpResult<T>> HttpRequestWithTokens<T>(string route, RequestDelegate requestDelegate)
        {
            // Get token
            Token token = new Token
            {
                AccessToken = await _tokenService.GetToken(),
                RefreshToken = await _tokenService.GetRefreshToken()
            };
            // Try http request with AccesToken in header
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.AccessToken}");
            var response = await requestDelegate(route);

            switch (response.StatusCode)
            {
                // If response status code is OK return response content
                case HttpStatusCode.OK:
                    var result = await response.Content.ReadFromJsonAsync<T>();
                    return new HttpResult<T> { Result = result, StatusCode = HttpStatusCode.OK };

                // If response status code is Unauthorized try get a new token through RefreshToken 
                case HttpStatusCode.Unauthorized:
                    var refreshTokenResponse = await _httpClient.PostAsJsonAsync("api/auth/refresh-token", token);

                    if (refreshTokenResponse.IsSuccessStatusCode)
                    {
                        var newToken = await refreshTokenResponse.Content.ReadFromJsonAsync<Token>();
                        if (newToken != null && newToken.AccessToken != null && newToken.RefreshToken != null)
                        {
                            // Save new token 
                            await _tokenService.SetToken(newToken);
                            // Send new http request again with the new AccessToken
                            _httpClient.DefaultRequestHeaders.Remove("Authorization");
                            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {newToken.AccessToken}");
                            response = await requestDelegate(route);

                            // Return response content
                            result = await response.Content.ReadFromJsonAsync<T>();
                            return new HttpResult<T> { Result = result, StatusCode = HttpStatusCode.OK };
                        }
                    }
                    return new HttpResult<T> { StatusCode = HttpStatusCode.Unauthorized };

                default:
                    return new HttpResult<T> { StatusCode = HttpStatusCode.BadRequest };
            }
        }

        public async Task<HttpResult<T>> HttpRequestWithTokens<T>(string route, T entity, RequestDelegateWithEntity requestDelegate)
        {
            if (entity == null) return new HttpResult<T> { StatusCode = HttpStatusCode.InternalServerError };
            // Get token
            Token token = new Token
            {
                AccessToken = await _tokenService.GetToken(),
                RefreshToken = await _tokenService.GetRefreshToken()
            };
            // Try http request with AccesToken in header
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.AccessToken}");
            var response = await requestDelegate(route, entity);

            switch (response.StatusCode)
            {
                // If response status code is OK return response content
                case HttpStatusCode.OK:
                    return new HttpResult<T> { Result = await response.Content.ReadFromJsonAsync<T>(), StatusCode = HttpStatusCode.OK };

                // If response status code is Unauthorized try get a new token through RefreshToken 
                case HttpStatusCode.Unauthorized:
                    var refreshTokenResponse = await _httpClient.PostAsJsonAsync("api/auth/refresh-token", token);

                    if (refreshTokenResponse.IsSuccessStatusCode)
                    {
                        var newToken = await refreshTokenResponse.Content.ReadFromJsonAsync<Token>();
                        if (newToken != null && newToken.AccessToken != null && newToken.RefreshToken != null)
                        {
                            // Save new token 
                            await _tokenService.SetToken(newToken);
                            // Send new http request again with the new AccessToken
                            _httpClient.DefaultRequestHeaders.Remove("Authorization");
                            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {newToken.AccessToken}");
                            response = await requestDelegate(route, entity);

                            // Return response content
                            return new HttpResult<T> { Result = await response.Content.ReadFromJsonAsync<T>(), StatusCode = HttpStatusCode.OK };
                        }
                    }
                    return new HttpResult<T> { StatusCode = HttpStatusCode.Unauthorized };

                default:
                    return new HttpResult<T> { StatusCode = HttpStatusCode.BadRequest };
            }
        }
    }
    public class HttpResult<T>
    {
        public T? Result { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }

    public delegate Task<HttpResponseMessage> RequestDelegate(string? route);
    public delegate Task<HttpResponseMessage> RequestDelegateWithEntity(string? route, Object entity);
}

