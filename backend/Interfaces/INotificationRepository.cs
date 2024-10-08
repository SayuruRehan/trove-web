// IT21470004 - BOPITIYA S. R. - Interface for Notification Repository

using backend.Models;

namespace backend.Interfaces
{
    public interface INotificationRepository
    {
        Task AddNotification(Notification notification);
        Task<IEnumerable<Notification>> GetNotificationsByCustomerId(string customerId);
    }
}
