using CleanEjdg.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanEjdg.Core.Domain.ValueTypes
{
    public class RegisterUserModel
    {
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = String.Empty;

        [Required(ErrorMessage = "The email address is required")]
        [StringLength(16, MinimumLength = 1)]
        public string UserName { get; set; } = String.Empty;

        [Required(ErrorMessage = "A password is required")]
        public string Password { get; set; } = String.Empty;
        
        [Required(ErrorMessage = "Repeating the is required")]
        [Compare(nameof(Password), ErrorMessage = "Passwords don't match.")]
        public string PasswordRepeat { get; set; } = String.Empty;

        public ApplicationUser ToApplicationUser()
        {
            ApplicationUser user = new ApplicationUser()
            {
                UserName = UserName,
                Email = Email
            };

            return user;
        }
    }
}