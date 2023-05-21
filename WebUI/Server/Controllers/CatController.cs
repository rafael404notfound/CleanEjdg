using Microsoft.AspNetCore.Mvc;

namespace WebUI.Server.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    public class CatController : ControllerBase {

        [HttpGet]
        public IActionResult GetCats(){
            return Ok();
        }
    }
}