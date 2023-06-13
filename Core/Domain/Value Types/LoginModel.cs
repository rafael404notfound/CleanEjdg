using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanEjdg.Core.Domain.ValueTypes
{
    public class LoginModel
    {
        public string message { get; set; } = String.Empty;
        public string email { get; set; } = String.Empty;
        public string jwtBearer { get; set; } = String.Empty;
        public bool success { get; set; }
    }
}
