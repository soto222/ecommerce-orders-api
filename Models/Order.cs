using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EcommerceOrdersApi.Models
{
    public class Order
    {
        [Key]
        public Guid OrderId { get; set; }

        [Required]
        public Guid CustomerId { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        public string OrderStatus { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        [Required]
        public string ShippingAddress { get; set; }

        [Required]
        public string BillingAddress { get; set; }

        public string Notes { get; set; }

        public List<OrderItem> OrderItems { get; set; }
    }
}
