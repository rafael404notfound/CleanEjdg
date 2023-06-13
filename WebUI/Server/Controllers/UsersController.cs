using CleanEjdg.Core.Application.Common;
using CleanEjdg.Core.Application.Repositories;
using CleanEjdg.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CleanEjdg.WebUI.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private UserRepository UserRepo;
        public UsersController(UserRepository userRepo)
        {
            UserRepo = userRepo;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserCredentials userCredentials)
        {
            IdentityResult result = await UserRepo.Create(userCredentials);
            if (result.Succeeded) return Ok(userCredentials);
            else return BadRequest(result.Errors);
        }
    }
}
