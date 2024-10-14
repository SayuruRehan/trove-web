using Microsoft.AspNetCore.Mvc;
using web_service.Models;
using web_service.Services;

namespace web_service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly PaymentService _paymentService;
        private readonly UserService _userService;

        public PaymentsController(PaymentService paymentService, UserService userService)
        {
            _paymentService = paymentService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var payments = await _paymentService.GetAsync();
                return Ok(payments);
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(500, new { message = "An error occurred while retrieving payments.", error = ex.Message });
            }
        }

        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var payment = await _paymentService.GetAsync(id);

                if (payment == null)
                {
                    return NotFound(new { message = "Payment not found." });
                }

                return Ok(payment);
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(500, new { message = "An error occurred while retrieving the payment.", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Payment newPayment)
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
                // Validate UserId exists
                var user = await _userService.GetAsync(newPayment.UserId);
                if (user == null)
                {
                    return BadRequest(new { message = "Invalid UserId: User not found." });
                }

                // Ensure PaymentId is generated by MongoDB
                newPayment.PaymentId = null; // Let MongoDB handle ID generation

                await _paymentService.CreateAsync(newPayment);
                return Ok(new { message = "Payment created successfully.", payment = newPayment });
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(500, new { message = "An error occurred while creating the payment.", error = ex.Message });
            }
        }


        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, [FromBody] Payment updatedPayment)
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
                var payment = await _paymentService.GetAsync(id);

                if (payment == null)
                {
                    return NotFound(new { message = "Payment not found." });
                }

                // Validate UserId exists
                var user = await _userService.GetAsync(updatedPayment.UserId);
                if (user == null)
                {
                    return BadRequest(new { message = "Invalid UserId: User not found." });
                }

                updatedPayment.PaymentId = payment.PaymentId;

                await _paymentService.UpdateAsync(id, updatedPayment);

                return Ok(new { message = "Payment updated successfully.", payment = updatedPayment });
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(500, new { message = "An error occurred while updating the payment.", error = ex.Message });
            }
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var payment = await _paymentService.GetAsync(id);

                if (payment == null)
                {
                    return NotFound(new { message = "Payment not found." });
                }

                await _paymentService.RemoveAsync(id);

                return Ok(new { message = "Payment deleted successfully." });
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(500, new { message = "An error occurred while deleting the payment.", error = ex.Message });
            }
        }
    }
}
