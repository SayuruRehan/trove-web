using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;

namespace backend.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderByIdAsync(string id);
        Task<Order> CreateOrderAsync(Order order);
        Task<Order> UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(string id);
    }
}
