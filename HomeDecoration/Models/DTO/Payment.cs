using System.ComponentModel.DataAnnotations;

namespace HomeDecoration.Models.DTO
{
    public class Payment
    {
        [Key]
        public int InvoiceId { get; set; }
        [Required]
        public Invoice? Invoice { get; set; }

        public decimal Amount { get; set; }

        public decimal Remainder { get; set; }

        public string? Payment_Type { get; set; }

        public decimal Discount { get; set; }

    }
}
