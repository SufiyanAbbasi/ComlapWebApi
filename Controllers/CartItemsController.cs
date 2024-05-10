using ComlapWebApi.Models;
using ComlapWebApi.Models.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComlapWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {
        private readonly ComlapDbContext context;

        public CartItemsController(ComlapDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartItem>>> GetCartItems()
        {
            var data = await context.CartItems.ToListAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CartItem>> GetCartItem(int id)
        {
            var cartItem = await context.CartItems.FindAsync(id);

            if (cartItem == null)
            {
                return NotFound();
            }

            return cartItem;
        }

        // POST: api/cartitems
        [HttpPost]
        public async Task<ActionResult<CartItem>> PostCartItem(CartItem cartItem)
        {
            context.CartItems.Add(cartItem);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCartItem), new { id = cartItem.Id }, cartItem);
        }

        // PUT: api/cartitems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCartItem(int id, CartItem cartItem)
        {
            if (id != cartItem.Id)
            {
                return BadRequest();
            }

            context.Entry(cartItem).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok(cartItem);
        }

        // DELETE: api/cartitems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCartItem(int id)
        {
            var cartItem = await context.CartItems.FindAsync(id);
            if (cartItem == null)
            {
                return NotFound();
            }

            context.CartItems.Remove(cartItem);
            await context.SaveChangesAsync();

            return NoContent();
        }


        private bool CartItemExists(int id)
        {
            return context.CartItems.Any(e => e.Id == id);
        }
    }
}
