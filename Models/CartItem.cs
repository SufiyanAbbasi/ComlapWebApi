using System.ComponentModel.DataAnnotations;

namespace ComlapWebApi.Models
{
    public class CartItem
    {
      
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public User User { get; set; }  // Navigation property
        public Product Product { get; set; }

        //public List<CartItem> CartItems { get; set; }
    }
}
