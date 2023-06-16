using CleanEjdg.Core.Application.Repositories;
using CleanEjdg.Core.Domain.Entities;
using CleanEjdg.Core.Domain.ValueTypes;
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
    public class AuthController : ControllerBase
    {   
        private UserRepository UserRepo { get; set; }
        private UserManager<IdentityUser> UserManager { get; set; }
        private IConfiguration Configuration { get; set; }

        public AuthController(UserRepository userRepo, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            UserManager = userManager;
            Configuration = configuration;
            UserRepo = userRepo;
        }

        [NonAction]
        public async Task<string> CreateJWT(IdentityUser user)
        {
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
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new JwtSecurityToken(
                issuer: Configuration.GetValue<String>("Jwt:Issuer"),
                audience: Configuration.GetValue<String>("Jwt:Audience"),
                claims: claims,
                expires: DateTime.Now.AddMinutes(720),
                signingCredentials: credentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            return await Task.FromResult(jwt);

        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserCredentials userCredentials)
        {
            IdentityResult result = await UserRepo.Create(userCredentials);
            if (result.Succeeded) return Ok(userCredentials);
            else return BadRequest(result.Errors);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Post([FromBody] UserCredentials userCredentials)
        {
            var user = await UserManager.FindByNameAsync(userCredentials.UserName);
            if (user is null || !await UserManager.CheckPasswordAsync(user, userCredentials.Password))
            {
                return BadRequest(new LoginResult { Message = "Unable to Log in", Success = false });
            }
            var token = await CreateJWT(user);
            Response.Cookies.Append("X-Access-Token", token, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
            Response.Cookies.Append("X-Username", user.UserName, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
            return Ok(new LoginResult { Message = "Login successful", JwtBearer = token, Email = userCredentials.Email ?? "", Success = true });
        }
    }
}
