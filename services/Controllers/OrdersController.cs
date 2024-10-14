using Microsoft.AspNetCore.Mvc;
using web_service.Models;
using web_service.Services;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace web_service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;
        private readonly PaymentService _paymentService;
        private readonly UserService _userService;
        private readonly ProductService _productService;

        public OrdersController(OrderService orderService, PaymentService paymentService, UserService userService, ProductService productService)
        {
            _orderService = orderService;
            _paymentService = paymentService;
            _userService = userService;
            _productService = productService;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var orders = await _orderService.GetAsync();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(500, new { message = "An error occurred while retrieving orders.", error = ex.Message });
            }
        }

        // GET: api/Orders/{id}
        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var order = await _orderService.GetAsync(id);

                if (order == null)
                {
                    return NotFound(new { message = "Order not found." });
                }

                return Ok(order);
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(500, new { message = "An error occurred while retrieving the order.", error = ex.Message });
            }
        }

        // GET: api/Orders/user/{userId}
        [HttpGet("user/{userId:length(24)}")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            try
            {
                var orders = await _orderService.GetByUserIdAsync(userId);

                if (orders == null || !orders.Any())
                {
                    return NotFound(new { message = "No orders found for this user." });
                }

                return Ok(orders);
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(500, new { message = "An error occurred while retrieving orders by UserId.", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Order newOrder)
        {
            newOrder.OrderId = null; // Ensure OrderId is autogenerated

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList();

                return BadRequest(new { message = "Validation failed.", errors });
            }

            try
            {
                // Validate UserId exists
                var user = await _userService.GetAsync(newOrder.UserId);
                if (user == null)
                {
                    return BadRequest(new { message = "Invalid UserId: User not found." });
                }

                // Validate PaymentId exists
                // var payment = await _paymentService.GetAsync(newOrder.PaymentId);
                // if (payment == null)
                // {
                //     return BadRequest(new { message = "Invalid PaymentId: Payment not found." });
                // }
                

                // Validate ProductIds exist
                List<string> invalidProductIds = new List<string>();
                foreach (var productId in newOrder.ProductIds)
                {
                    var product = await _productService.GetAsync(productId);
                    if (product == null)
                    {
                        invalidProductIds.Add(productId);
                    }
                }

                if (invalidProductIds.Any())
                {
                    return BadRequest(new { message = "Invalid ProductIds: The following products were not found.", invalidProductIds });
                }

                await _orderService.CreateAsync(newOrder);
                return Ok(new { message = "Order created successfully.", order = newOrder });
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(500, new { message = "An error occurred while creating the order.", error = ex.Message });
            }
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, [FromBody] Order updatedOrder)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList();

                return BadRequest(new { message = "Validation failed.", errors });
            }

            try
            {
                var existingOrder = await _orderService.GetAsync(id);

                if (existingOrder == null)
                {
                    return NotFound(new { message = "Order not found." });
                }

                // Validate UserId exists
                var user = await _userService.GetAsync(updatedOrder.UserId);
                if (user == null)
                {
                    return BadRequest(new { message = "Invalid UserId: User not found." });
                }

                // Validate PaymentId exists (if it has changed)
                // if (updatedOrder.PaymentId != existingOrder.PaymentId)
                // {
                //     var payment = await _paymentService.GetAsync(updatedOrder.PaymentId);
                //     if (payment == null)
                //     {
                //         return BadRequest(new { message = "Invalid PaymentId: Payment not found." });
                //     }
                // }

                // Validate ProductIds exist
                List<string> invalidProductIds = new List<string>();
                foreach (var productId in updatedOrder.ProductIds)
                {
                    var product = await _productService.GetAsync(productId);
                    if (product == null)
                    {
                        invalidProductIds.Add(productId);
                    }
                }

                if (invalidProductIds.Any())
                {
                    return BadRequest(new { message = "Invalid ProductIds: The following products were not found.", invalidProductIds });
                }

                updatedOrder.OrderId = existingOrder.OrderId;

                await _orderService.UpdateAsync(id, updatedOrder);

                return Ok(new { message = "Order updated successfully.", order = updatedOrder });
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(500, new { message = "An error occurred while updating the order.", error = ex.Message });
            }
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var order = await _orderService.GetAsync(id);

                if (order == null)
                {
                    return NotFound(new { message = "Order not found." });
                }

                await _orderService.RemoveAsync(id);

                return Ok(new { message = "Order deleted successfully." });
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(500, new { message = "An error occurred while deleting the order.", error = ex.Message });
            }
        }
    }
}
