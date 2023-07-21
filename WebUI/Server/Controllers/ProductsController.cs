using Microsoft.AspNetCore.Mvc;
using CleanEjdg.Core.Application.Services;
using CleanEjdg.Core.Domain.Entities;
using CleanEjdg.Core.Application.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WebUI.Server.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        IRepositoryBase<Product> ProductRepo;

        public ProductsController(IRepositoryBase<Product> productRepo)
        {
            ProductRepo = productRepo;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            IEnumerable<Product> Products = ProductRepo.GetAll();
            if (Products.Count() != 0 && Products != null)
            {
                return Ok(Products);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            Product product = await ProductRepo.Get(id);
            if (product.Id == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok(product);
            }
        }

        [HttpPost]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> SaveProduct([FromBody] Product product)
        {

            ProductRepo.Create(product);
            return Ok(product);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                await ProductRepo.Delete(id);
                return Ok();
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            try
            {
            await ProductRepo.Update(product);
            return Ok(product);
            } catch
            {
                return NotFound();
            }            
        }
    }
}