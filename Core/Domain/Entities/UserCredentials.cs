using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CleanEjdg.Core.Domain.Entities
{
    public class UserCredentials 
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        public IdentityUser ToIdentityUser()
        {
            IdentityUser user = new IdentityUser()
            {
                UserName = UserName,
                Email = Email
            };

            return user;
        }
    }
}
