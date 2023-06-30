using CleanEjdg.Core.Domain.ValueTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanEjdg.Core.Application.Services
{ 
    public interface ITokenService
    {
        Task<string> GetToken();
        Task SetToken(LoginResult token);
        Task DeleteToken();
        Task<LoginResult> CreateToken(UserCredentials credentials);

    }
}
