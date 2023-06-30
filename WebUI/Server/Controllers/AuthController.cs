using CleanEjdg.Core.Application.Repositories;
using CleanEjdg.Core.Domain.Entities;
using CleanEjdg.Core.Domain.ValueTypes;
using CleanEjdg.Core.Domain.Dtos;
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
                //issuer: Configuration.GetValue<String>("Jwt:Issuer"),
                //audience: Configuration.GetValue<String>("Jwt:Audience"),
                claims: claims,
                expires: DateTime.Now.AddMinutes(720),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            //return await Task.FromResult(jwt);

        }
        
        [HttpPost]
        [Route("validate")]
        public IActionResult ValidateToken([FromBody]string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<String>("Jwt:Key")));

            try
            {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = true
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userEmail = jwtToken.Claims.First(x => x.Type == ClaimTypes.Email).Value;

                return Ok(userEmail);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("roles")]
        public IActionResult Roles(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<String>("Jwt:Key")));

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = true
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var roles = jwtToken.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value);

                return Ok(roles.ToArray());
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserCredentials userCredentials)
        {   
            //Todo: check if model is vlaid, check taht a user with the same email doesnt already exsist in db
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
            return Ok(new LoginResult { Message = "Login successful", JwtBearer = token, Email = userCredentials.Email ?? "", Success = true });
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var userDtos = new List<UserDto>();
            List<string> userRoles = new List<string>();
            var users = UserManager.Users.ToList();

            foreach (var user in users)
            {
                userRoles = await UserManager.GetRolesAsync(user) as List<string> ?? new List<string>();
                userDtos.Add(new UserDto(user, userRoles));
            }
            if (userDtos.ToArray().Length != 0) return Ok(userDtos);
            else return NotFound();
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            var userRoles = await UserManager.GetRolesAsync(user) as List<string> ?? new List<string>();

            if (user != null && userRoles != null) return Ok(new UserDto(user, userRoles));
            else return NotFound();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody]UserDto userDto)
        {
            var user = await UserManager.FindByIdAsync(userDto.Id);
            if (user != null)
            {
                await UserManager.RemoveFromRolesAsync(user, new string[] { "CatEditor", "OrderEditor", "ItemEditor" });
                await UserManager.AddToRolesAsync(user, userDto.Roles);
                return Ok();

            } else
            {
                return NotFound();
            }
        }
    }
}
