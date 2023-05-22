using Microsoft.AspNetCore.Mvc;
using CleanEjdg.Core.Application.Services;
using CleanEjdg.Core.Domain.Entities;

namespace WebUI.Server.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    public class CatsController : ControllerBase {
        ICatService CatService;

        public CatsController(ICatService catService) {
            CatService = catService;
        }

        [HttpGet]
        public IActionResult GetCats(){
            IEnumerable<Cat> Cats = CatService.GetAllCats();
            if(Cats.Count() != 0)
            {
                return Ok(Cats);
            } else
            {
                return BadRequest();
            }            
        }
    }
}