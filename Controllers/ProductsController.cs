using ComlapWebApi.Models;
using ComlapWebApi.Models.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComlapWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ComlapDbContext context;

        public ProductsController(ComlapDbContext context)
        {
            this.context = context;
        }

        //[HttpGet]
        //public async Task <ActionResult<List<Product>>> GetProducts()
        //{ 
        //    var data = await context.Products.ToListAsync();
        //    return Ok(data);
        //}

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery] string category)
        {
            var products = await context.Products
                                         .Where(p => p.Category.ToLower() == category.ToLower())
                                         .ToListAsync();
            if (!products.Any())
            {
                return NotFound("No products found in the given category.");
            }
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task <ActionResult<Product>> GetProductById(int id)
        {
            var product = await context.Products.FindAsync(id);
            if(product == null)
            { 
              return NotFound();
            }
            return product;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product prod)
        {
            await context.Products.AddAsync(prod);
            await context.SaveChangesAsync();
            return Ok(prod);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Product>> UpdateProduct(int id, Product prod)
        {
           if(id  != prod.Id)
            {
                return BadRequest();
            }
           context.Entry(prod).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok(prod);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
          var product = await context.Products.FindAsync(id);

            if( product == null)
            {
                return NotFound();
            }
            context.Products.Remove(product);
            await context.SaveChangesAsync();
            return Ok();
        }


    }
}
