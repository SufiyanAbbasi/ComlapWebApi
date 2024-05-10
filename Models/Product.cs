using System.ComponentModel.DataAnnotations;

namespace ComlapWebApi.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public string ImageUrl { get; set; }

        public string Category { get; set; }



    }
}
