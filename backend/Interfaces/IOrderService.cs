using System.Collections.Generic;
using System.Threading.Tasks;
using backend.DTOs;

namespace backend.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
        Task<OrderDto> GetOrderByIdAsync(string id);
        Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto);
        Task<OrderDto> UpdateOrderAsync(UpdateOrderDto updateOrderDto);
        Task DeleteOrderAsync(string id);
        Task<IEnumerable<OrderItemDto>> GetSubOrdersByVendorIdAsync(string vendorId);
        Task<OrderItemDto> UpdateOrderItemAsync(UpdateOrderItemDto updateOrderItemDto);
        Task<IEnumerable<OrderDto>> GetAllOrdersWithItemsAsync();

    }
}