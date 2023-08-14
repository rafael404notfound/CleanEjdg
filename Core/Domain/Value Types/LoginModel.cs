using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanEjdg.Core.Domain.ValueTypes
{
    public class LoginResult
    {
        public string Message { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string JwtBearer { get; set; } = String.Empty;
        public string UserName { get; set; } = String.Empty;
        public bool Success { get; set; } = false;
    }
}
