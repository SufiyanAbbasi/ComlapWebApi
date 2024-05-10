using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ComlapWebApi.Models.Data
{
    public class ComlapDbContext : DbContext 
    {
        public ComlapDbContext(DbContextOptions<ComlapDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Your Database Connection String Here"); // Replace with actual connection string
        //}

    }
}
