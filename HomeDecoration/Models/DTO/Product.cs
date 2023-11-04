using System.ComponentModel.DataAnnotations;

namespace HomeDecoration.Models.DTO
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }

        public string? Description { get; set; }

        public int Price { get; set; }

        public int Stock { get; set; }

        public string? Status { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
