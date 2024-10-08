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

        public async Task<IEnumerable<Notification>> GetNotificationsByCustomerId(string customerId)
        {
            return await _notificationRepository.GetNotificationsByCustomerId(customerId);
        }
    }
}
