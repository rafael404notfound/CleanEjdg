using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;
using CleanEjdg.Core.Domain.Entities;

namespace CleanEjdg.Core.Domain.Dtos
{
    public class UserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = String.Empty;
        public List<string> Roles { get; set; } = new List<string>();
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public UserDto(ApplicationUser user, List<string> roles)
        {
            Id = user.Id;
            UserName = user.UserName;
            Email = user.Email;
            Roles = roles;
            RefreshToken = user.RefreshToken;
            RefreshTokenExpiryTime = user.RefreshTokenExpiryTime;
        }

        [JsonConstructor]
        public UserDto() { }
    }
}
