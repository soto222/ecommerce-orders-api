using System;
using System.ComponentModel.DataAnnotations;

namespace EcommerceOrdersApi.Models
{
    public class Product
    {
        [Key]
        public Guid ProductId { get; set; }

        [Required]
        [MaxLength(50)]
        public string SKU { get; set; }

        [Required]
        [MaxLength(50)]
        public string InternalCode { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public decimal CurrentUnitPrice { get; set; }

        [Required]
        public int StockQuantity { get; set; }
    }
}
