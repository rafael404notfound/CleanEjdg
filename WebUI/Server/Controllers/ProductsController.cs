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
        public IActionResult GetCats()
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

    }
}