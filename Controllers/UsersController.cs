using ComlapWebApi.Models;
using ComlapWebApi.Models.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ComlapWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ComlapDbContext context;

        public UsersController(ComlapDbContext context)
        {
            this.context = context;
        }

        // GET: api/users
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        //{
        //    return await context.Users.ToListAsync();
        //}
        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await context.Users
                                     .Select(user => new UserDto
                                     {
                                         Id = user.Id,
                                         Name = user.Name,
                                         Email = user.Email
                                     }).ToListAsync();
            return users;
        }

        // GET: api/users/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<User>> GetUser(int id)
        //{
        //    var user = await context.Users.FindAsync(id);

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return user;
        //}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var userDto = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };

            return userDto;
        }

        //[HttpPost("register")]
        //public async Task<ActionResult<User>> RegisterUser(User user)
        //{
        //    context.Users.Add(user);
        //    await context.SaveChangesAsync();

        //    return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        //}
        [HttpPost("register")]
        public async Task<ActionResult<User>> RegisterUser([FromBody] RegisterDto registerDto)
        {
            if (await context.Users.AnyAsync(u => u.Email == registerDto.Email))
                return BadRequest("Email already in use.");

            var user = new User
            {
                Name = registerDto.Name,
                Email = registerDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password)
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }


        // POST: api/users/login
        //[HttpPost("login")]
        //public async Task<ActionResult<User>> LoginUser(User user)
        //{

        //    return Ok(user);
        //}

        [HttpPost("login")]
        public async Task<ActionResult<User>> LoginUser([FromBody] LoginDto loginDto)
        {
            var user = await context.Users.SingleOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null)
                return NotFound("User not found.");

            bool passwordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);
            if (!passwordValid)
                return Unauthorized("Invalid password.");

            return Ok(user);
        }



        // PUT: api/users/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateUser(int id, User user)
        //{
        //    if (id != user.Id)
        //    {
        //        return BadRequest();
        //    }

        //    context.Entry(user).State = EntityState.Modified;

        //    try
        //    {
        //        await context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UserExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserDto userDto)
        {
            if (id != userDto.Id)
            {
                return BadRequest();
            }

            var user = await context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Update user properties
            user.Name = userDto.Name;
            user.Email = userDto.Email;

            // Save changes
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            context.Users.Remove(user);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return context.Users.Any(e => e.Id == id);
        }

    }
}
