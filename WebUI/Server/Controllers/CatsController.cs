using Microsoft.AspNetCore.Mvc;
using CleanEjdg.Core.Application.Services;
using CleanEjdg.Core.Domain.Entities;
using CleanEjdg.Core.Application.Common;

namespace WebUI.Server.Controllers {

    [ApiController]
    [Route("api/[controller]")]
    public class CatsController : ControllerBase {
        IRepositoryBase<Cat> CatRepo;
        IDateTimeServer DateTimeServer;

        public CatsController(IRepositoryBase<Cat> catRepo, IDateTimeServer dateTimeServer) {
            CatRepo = catRepo;
            DateTimeServer = dateTimeServer;
        }

        [HttpGet]
        public IActionResult GetCats(){
            IEnumerable<Cat> Cats = CatRepo.GetAll();
            if (Cats.Count() != 0 && Cats != null)
            {
                return Ok(Cats);
            } else
            {
                return NotFound();
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
           
            Cat cat = target.ToCat();
            await CatRepo.Create(cat);
            return Ok(cat);
                        
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCat(int id)
        {
            try
            {
                await CatRepo.Delete(id);
                return Ok();
            }
            catch
            {
                return NotFound(DateTimeServer.Now);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCat([FromBody]Cat cat)
        {
            try
            {
                await CatRepo.Update(cat);
                return Ok(cat);
            } catch
            {
                return NotFound();
            }            
        }
    }
}