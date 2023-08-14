using CleanEjdg.Core.Domain.ValueTypes;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CleanEjdg.Core.Application.Services
{
    public class AppState
    {
        public string Username { get ; private set; } = string.Empty;

        /*public async Task<string> GetUsername()
        {
            var token = await TokenService.GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                var content = new StringContent(token);
                var httpClient = new HttpClient();
                var result = await httpClient.PostAsync("https://localhost:7137/api/auth/validate", content);
                if (result.IsSuccessStatusCode)
                {
                    var loginResult = JsonSerializer.Deserialize<LoginResult>(result.Content.ReadAsStream());
                    if (loginResult != null) return loginResult.UserName;
                }
            }
            return string.Empty;
        }*/
        public void UpdateUsername(ComponentBase source, string username)
        {
            this.Username = username;
            NotifyStateChanged(source, "Username");
        }

        public event Action<ComponentBase, string> StateChanged;

        private void NotifyStateChanged(ComponentBase source, string property) => StateChanged?.Invoke(source, property);


    }
}
