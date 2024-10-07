using backend.Interfaces;
using backend.Models;
using MongoDB.Driver;

namespace backend.Repositories
{
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

        public async Task<IEnumerable<Notification>> GetNotificationsByCustomerId(string customerId)
        {
            var filter = Builders<Notification>.Filter.Eq(n => n.CustomerId, customerId);
            return await _notifications.Find(filter).ToListAsync();
        }
    }
}
