using System;
using System.Collections.Generic;

namespace EcommerceOrdersApi.DTOs
{
    public class CreateOrderDto
    {
        public Guid CustomerId { get; set; }
        public string ShippingAddress { get; set; }
        public string BillingAddress { get; set; }
        public string? Notes { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }
}
