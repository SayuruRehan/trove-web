// IT21470004 - BOPITIYA S. R. - Notification Service

using backend.DTOs;
using backend.Interfaces;
using backend.Models;

namespace backend.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        //initialize the repository
        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        // Add notification
        public async Task AddNotification(NotificationDTO notificationDTO)
        {
            var notification = new Notification
            {
                CustomerId = notificationDTO.CustomerId,
                SenderId = notificationDTO.SenderId,
                Title = notificationDTO.Title,
                Description = notificationDTO.Description
            };
            await _notificationRepository.AddNotification(notification);
        }

        // Get notifications by customer id
        public async Task<IEnumerable<Notification>> GetNotificationsByCustomerId(string customerId)
        {
            return await _notificationRepository.GetNotificationsByCustomerId(customerId);
        }
    }
}
