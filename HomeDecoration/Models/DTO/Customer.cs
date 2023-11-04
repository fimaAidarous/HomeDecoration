using System.ComponentModel.DataAnnotations;

namespace HomeDecoration.Models.DTO
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }

        public string? Address { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
