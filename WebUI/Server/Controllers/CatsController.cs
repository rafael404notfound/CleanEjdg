using Microsoft.AspNetCore.Mvc;
using CleanEjdg.Core.Application.Services;
using CleanEjdg.Core.Domain.Entities;
using CleanEjdg.Core.Application.Common;

namespace WebUI.Server.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    public class CatsController : ControllerBase {
        IRepositoryBase<Cat> CatRepo;

        public CatsController(IRepositoryBase<Cat> catRepo) {
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

        [HttpPost]
        public async Task<IActionResult> SaveCat([FromBody]CatBindingTarget target)
        {        
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            } else
            {
                Cat cat = target.ToCat();
                await CatRepo.Create(cat);
                return Ok(cat);
            }            
        }

        [HttpDelete("{id}")]
        public async Task DeleteCat(int id)
        {            
            await CatRepo.Delete(id);
        }

        [HttpPut]
        public async Task UpdateCat([FromBody]Cat cat)
        {
            await CatRepo.Update(cat);
        }
    }
}