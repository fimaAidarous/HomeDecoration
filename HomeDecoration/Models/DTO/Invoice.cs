using System.ComponentModel.DataAnnotations;

namespace HomeDecoration.Models.DTO
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }
        [Required]

        public int OrderId { get; set; }

        public Order? Order { get; set; }

        public Payment? Payment { get; set; }

        public int Total_Amount { get; set; }

        public string? Payment_Status { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
