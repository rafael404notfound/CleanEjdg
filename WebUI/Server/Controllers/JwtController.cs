using CleanEjdg.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebUI.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JwtController : ControllerBase
    {   
        private UserManager<IdentityUser> UserManager { get; set; }
        private IConfiguration Configuration { get; set; }

        public JwtController(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            UserManager = userManager;
            Configuration = configuration; 
        }
        [HttpPost]
        public async Task<IResult> CreateJwt([FromBody]UserCredentials userCredentials)
        {
            // Verificamos credenciales con Identity
            var user = await UserManager.FindByNameAsync(userCredentials.UserName);

            if (user is null || !await UserManager.CheckPasswordAsync(user, userCredentials.Password))
            {
                return Results.Forbid();
            }

            var roles = await UserManager.GetRolesAsync(user);

            // Generamos un token según los claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
           
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<String>("Jwt:Key")));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new JwtSecurityToken(
                issuer: Configuration.GetValue<String>("Jwt:Issuer"),
                audience: Configuration.GetValue<String>("Jwt:Audience"),
                claims: claims,
                expires: DateTime.Now.AddMinutes(720),
                signingCredentials: credentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

            return Results.Ok(new
            {
                AccessToken = jwt
            });
        }
    }
}
