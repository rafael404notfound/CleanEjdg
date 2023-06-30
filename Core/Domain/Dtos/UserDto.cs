using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace CleanEjdg.Core.Domain.Dtos
{
    public class UserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = String.Empty;
        public List<string> Roles { get; set; } = new List<string>();
        public UserDto(IdentityUser user, List<string> roles)
        {
            Id = user.Id;
            UserName = user.UserName;
            Email = user.Email;
            Roles = roles;
        }

        [JsonConstructor]
        public UserDto() { }
    }
}
