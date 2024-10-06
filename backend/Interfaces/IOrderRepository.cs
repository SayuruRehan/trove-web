using System.Collections.Generic;
using System.Threading.Tasks;
using backend.DTOs;
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
        Task<List<string>> AddOrderItemsAsync(List<OrderItem> orderItems);
        Task<IEnumerable<OrderItemDto>> GetSubOrdersByVendorIdAsync(string vendorId);
        Task<OrderItem> UpdateOrderItemAsync(UpdateOrderItemDto updateOrderItemDto);
        Task<IEnumerable<OrderDto>> GetAllOrdersWithItemsAsync();


    }
}