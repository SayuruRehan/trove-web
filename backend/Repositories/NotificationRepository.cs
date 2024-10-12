// IT21470004 - BOPITIYA S. R. - NotificationRepository

using backend.Interfaces;
using backend.Models;
using MongoDB.Driver;

namespace backend.Repositories
{
    // NotificationRepository class implements INotificationRepository interface
    public class NotificationRepository : INotificationRepository
    {
        private readonly IMongoCollection<Notification> _notifications;

        public NotificationRepository(IMongoDatabase database)
        {
            _notifications = database.GetCollection<Notification>("Notifications");
        }

        public async Task AddNotification(Notification notification)
        {
            await _notifications.InsertOneAsync(notification);
        }

        // Get all notifications by customer id
        public async Task<IEnumerable<Notification>> GetNotificationsByCustomerId(string customerId)
        {
            var filter = Builders<Notification>.Filter.Eq(n => n.CustomerId, customerId);
            return await _notifications.Find(filter).ToListAsync();
        }
    }
}
