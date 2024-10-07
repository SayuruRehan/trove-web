using backend.DTOs;
using backend.Models;

namespace backend.Interfaces
{
    public interface INotificationService
    {
        Task AddNotification(NotificationDTO notificationDTO);
        Task<IEnumerable<Notification>> GetNotificationsByCustomerId(string customerId);
    }
}
