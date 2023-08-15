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
using System.Security.Cryptography;

namespace WebUI.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {   
        private UserRepository UserRepo { get; set; }
        private UserManager<ApplicationUser> UserManager { get; set; }
        private IConfiguration Configuration { get; set; }

        public AuthController(UserRepository userRepo, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            UserManager = userManager;
            Configuration = configuration;
            UserRepo = userRepo;
        }

        [NonAction]
        public async Task<string> CreateJWT(ApplicationUser user)
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
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            //return await Task.FromResult(jwt);

        }
        [NonAction]
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        [HttpGet]
        [Route("validate/{token}")]
        public IActionResult ValidateToken(string token)
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
                var result = new LoginResult
                {
                    Email = jwtToken.Claims.First(x => x.Type == ClaimTypes.Email).Value,
                    UserName = jwtToken.Claims.First(x => x.Type == ClaimTypes.Name).Value,
                };

                return Ok(result);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(Token token)
        {
            if (token is null)
            {
                return BadRequest("Invalid client request");
            }

            string? accessToken = token.AccessToken;
            string? refreshToken = token.RefreshToken;

            var principal = GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
            {
                return BadRequest("Invalid access token or refresh token");
            }

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            string username = principal.Identity.Name;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            var user = await UserManager.FindByNameAsync(username);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid access token or refresh token");
            }

            var newAccessToken = await CreateJWT(user);
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.SpecifyKind(user.RefreshTokenExpiryTime, DateTimeKind.Utc);
            await UserManager.UpdateAsync(user);

            return Ok(new Token
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
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
        public async Task<IActionResult> Register([FromBody] RegisterUserModel userCredentials)
        {
            //Todo: check if model is vlaid, check taht a user with the same email doesnt already exsist in db
            var userExists = await UserManager.FindByEmailAsync(userCredentials.Email);
            if (userExists != null || !ModelState.IsValid)
            {
                return BadRequest();
            }
            IdentityResult result = await UserManager.CreateAsync(userCredentials.ToApplicationUser(), userCredentials.Password);
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
            //var token = await CreateJWT(user);
            var token = new Token
            {
                AccessToken = await CreateJWT(user),
                RefreshToken = GenerateRefreshToken()
            };

            _ = int.TryParse(Configuration["Jwt:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

            user.RefreshToken = token.RefreshToken;
            user.RefreshTokenExpiryTime = DateTime.SpecifyKind(DateTime.Now.AddDays(refreshTokenValidityInDays), DateTimeKind.Utc);

            await UserManager.UpdateAsync(user);
            return Ok(new LoginResult { Message = "Login successful", Token = token, Email = userCredentials.Email ?? "", UserName = userCredentials.UserName, Success = true });
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
                await UserManager.RemoveFromRolesAsync(user, new string[] { UserRoles.CatEditor, UserRoles.OrderEditor, UserRoles.ItemEditor, UserRoles.Admin });
                await UserManager.AddToRolesAsync(user, userDto.Roles);
                return Ok();

            } else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("Get-Principal-From-Token")]
        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;

        }
    }
}
