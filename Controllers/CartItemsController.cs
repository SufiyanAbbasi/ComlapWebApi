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
        //[HttpPost]
        //public async Task<ActionResult<CartItem>> PostCartItem(CartItem cartItem)
        //{
        //    context.CartItems.Add(cartItem);
        //    await context.SaveChangesAsync();

        //    return CreatedAtAction(nameof(GetCartItem), new { id = cartItem.Id }, cartItem);
        //}
        //[HttpPost]
        //public async Task<ActionResult<CartItem>> PostCartItem(CartItem cartItem)
        //{
        //    if (cartItem == null || cartItem.UserId <= 0 || cartItem.ProductId <= 0 || cartItem.Quantity <= 0)
        //    {
        //        return BadRequest("Invalid cart item data.");
        //    }

        //    context.CartItems.Add(cartItem);
        //    await context.SaveChangesAsync();

        //    return CreatedAtAction(nameof(GetCartItem), new { id = cartItem.Id }, cartItem);
        //}
        [HttpPost]
        public async Task<ActionResult<CartItem>> PostCartItem(CartItem cartItem)
        {
            if (cartItem == null || cartItem.UserId <= 0 || cartItem.ProductId <= 0 || cartItem.Quantity <= 0)
            {
                return BadRequest("Invalid cart item data.");
            }

            // Ensure the User and Product exist
            var userExists = await context.Users.AnyAsync(u => u.Id == cartItem.UserId);
            var productExists = await context.Products.AnyAsync(p => p.Id == cartItem.ProductId);

            if (!userExists)
            {
                return BadRequest("User not found.");
            }

            if (!productExists)
            {
                return BadRequest("Product not found.");
            }

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

        // GET: api/cartitems/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<CartItem>>> GetCartItemsByUser(int userId)
        {
            var cartItems = await context.CartItems
                                         .Where(ci => ci.UserId == userId)
                                         .Include(ci => ci.Product) // Optionally include product details
                                         .ToListAsync();

            if (cartItems == null)
            {
                return NotFound();
            }

            return Ok(cartItems);
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
