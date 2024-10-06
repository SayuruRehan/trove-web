using backend.DTOs;
using backend.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrderById(string id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            var createdOrder = await _orderService.CreateOrderAsync(createOrderDto);
            return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.Id }, createdOrder);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<OrderDto>> UpdateOrder(string id, [FromBody] UpdateOrderDto updateOrderDto)
        {
            if (id != updateOrderDto.Id) return BadRequest("Order ID mismatch.");
            var updatedOrder = await _orderService.UpdateOrderAsync(updateOrderDto);
            if (updatedOrder == null) return NotFound();
            return Ok(updatedOrder);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(string id)
        {
            try
            {
                await _orderService.DeleteOrderAsync(id);
                return Ok(new { Message = $"Order with ID {id} has been deleted successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An unexpected error occurred.", Details = ex.Message });
            }
        }

        [HttpGet("vendor/{vendorId}/suborders")]
        public async Task<ActionResult<IEnumerable<OrderItemDto>>> GetSubOrdersByVendor(string vendorId)
        {
            var subOrders = await _orderService.GetSubOrdersByVendorIdAsync(vendorId);

            if (subOrders == null || !subOrders.Any())
            {
                return NotFound(new { Error = $"No sub-order items found for vendor ID {vendorId}." });
            }

            return Ok(subOrders);
        }

        [HttpPut("orderitems/{id}")]
        public async Task<ActionResult<OrderItemDto>> UpdateOrderItem(string id, [FromBody] UpdateOrderItemDto updateOrderItemDto)
        {
            if (id != updateOrderItemDto.Id) return BadRequest("OrderItem ID mismatch.");

            try
            {
                var updatedOrderItem = await _orderService.UpdateOrderItemAsync(updateOrderItemDto);
                return Ok(updatedOrderItem);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An unexpected error occurred.", Details = ex.Message });
            }
        }

        [HttpGet("items")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllOrdersWithItems()
        {
            var orders = await _orderService.GetAllOrdersWithItemsAsync();
            return Ok(orders);
        }


    }
}
