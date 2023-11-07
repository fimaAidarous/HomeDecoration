using System.ComponentModel.DataAnnotations;

namespace HomeDecoration.Models.DTO
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CustomerId { get; set; }

        public Customer? Customer { get; set; }

        public int ProductId { get; set; }

        public Product? Product { get; set; }

        public DateTime Order_Date { get; set; }

        public int Total_Amount { get; set; }

        public int Quantity { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
