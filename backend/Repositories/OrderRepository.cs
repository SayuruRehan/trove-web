using backend.Interfaces;
using backend.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IMongoCollection<Order> _orders;

        public OrderRepository(IMongoDatabase database)
        {
            _orders = database.GetCollection<Order>("Orders");
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orders.Find(FilterDefinition<Order>.Empty).ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(string id)
        {
            var filter = Builders<Order>.Filter.Eq(o => o.Id, id);
            return await _orders.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            await _orders.InsertOneAsync(order);
            return order;
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            var filter = Builders<Order>.Filter.Eq(o => o.Id, order.Id);
            await _orders.ReplaceOneAsync(filter, order);
            return order;
        }

        public async Task DeleteOrderAsync(string id)
        {
            var filter = Builders<Order>.Filter.Eq(o => o.Id, id);
            await _orders.DeleteOneAsync(filter);
        }
    }
}
