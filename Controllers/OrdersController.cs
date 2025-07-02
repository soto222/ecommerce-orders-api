using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EcommerceOrdersApi.Data;
using EcommerceOrdersApi.Models;
using EcommerceOrdersApi.DTOs;

namespace EcommerceOrdersApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
        {
            if (dto.OrderItems == null || !dto.OrderItems.Any())
                return BadRequest("La orden debe contener al menos un producto.");

            var orderItems = new List<OrderItem>();
            decimal total = 0;

            foreach (var item in dto.OrderItems)
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == item.ProductId);

                if (product == null)
                    return BadRequest($"Producto no encontrado: {item.ProductId}");

                if (product.StockQuantity < item.Quantity)
                    return BadRequest($"Stock insuficiente para el producto {product.Name}");

                var orderItem = new OrderItem
                {
                    OrderItemId = Guid.NewGuid(),
                    ProductId = product.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = product.CurrentUnitPrice,
                    Subtotal = item.Quantity * product.CurrentUnitPrice
                };

                product.StockQuantity -= item.Quantity;

                orderItems.Add(orderItem);
                total += orderItem.Subtotal;
            }

            var order = new Order
            {
                OrderId = Guid.NewGuid(),
                CustomerId = dto.CustomerId,
                OrderDate = DateTime.UtcNow,
                OrderStatus = "Pending",
                ShippingAddress = dto.ShippingAddress,
                BillingAddress = dto.BillingAddress,
                Notes = dto.Notes,
                TotalAmount = total,
                OrderItems = orderItems
            };

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrderById), new { id = order.OrderId }, order);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(Guid id, [FromBody] UpdateOrderStatusDto dto)
        {
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == id);
            if (order == null)
                return NotFound($"Orden con ID {id} no encontrada.");

            var allowedStatuses = new[] { "Pending", "Processing", "Shipped", "Delivered", "Cancelled" };
            if (!allowedStatuses.Contains(dto.NewStatus))
                return BadRequest($"Estado inválido. Estados permitidos: {string.Join(", ", allowedStatuses)}");

            order.OrderStatus = dto.NewStatus;
            await _context.SaveChangesAsync();

            return Ok(order);
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders(
            [FromQuery] string? status,
            [FromQuery] Guid? customerId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .AsQueryable();

            if (!string.IsNullOrEmpty(status))
                query = query.Where(o => o.OrderStatus == status);

            if (customerId.HasValue)
                query = query.Where(o => o.CustomerId == customerId);

            var totalItems = await query.CountAsync();
            var orders = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(new
            {
                totalItems,
                pageNumber,
                pageSize,
                data = orders
            });
        }
    }
}
