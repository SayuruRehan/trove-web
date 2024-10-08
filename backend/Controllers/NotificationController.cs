// IT21470004 - BOPITIYA S. R. -NOTIFICATION CONTROLLER

using backend.DTOs;
using backend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // Add notification
        [HttpPost("add")]
        public async Task<IActionResult> AddNotification([FromBody] NotificationDTO notificationDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _notificationService.AddNotification(notificationDTO);
            return Ok();
        }

        // Get all notifications
        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetNotificationsByCustomerId(string customerId)
        {
            var notifications = await _notificationService.GetNotificationsByCustomerId(customerId);
            if (notifications == null || !notifications.Any())
                return NotFound();

            return Ok(notifications);
        }
    }
}
