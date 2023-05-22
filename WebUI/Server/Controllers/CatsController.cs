using Microsoft.AspNetCore.Mvc;
using CleanEjdg.Core.Application.Services;
using CleanEjdg.Core.Domain.Entities;
using CleanEjdg.Core.Application.Repositories;

namespace WebUI.Server.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    public class CatsController : ControllerBase {
        ICatRepository CatRepo;

        public CatsController(ICatRepository catRepo) {
            CatRepo = catRepo;
        }

        [HttpGet]
        public IActionResult GetCats(){
            IEnumerable<Cat> Cats = CatRepo.GetAll();
            if (Cats.Count() != 0)
            {
                return Ok(Cats);
            } else
            {
                return BadRequest();
            }            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCat(int id)
        {
            Cat cat = await CatRepo.Get(id);
            if (cat.Id == 0)
            {
                return NotFound();
            } else
            {
                return Ok(cat);
            }
        }
    }
}