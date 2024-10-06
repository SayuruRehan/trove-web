using backend.DTOs;
using backend.Interfaces;
using backend.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // Get all orders
        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllOrdersAsync();

            // Use a mapping function to improve code reuse
            return orders.Select(MapToOrderDto);
        }

        // Get order by ID
        public async Task<OrderDto> GetOrderByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;

            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null) return null;

            return MapToOrderDto(order);
        }

        // Create a new order
        public async Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            if (createOrderDto == null)
                throw new ArgumentNullException(nameof(createOrderDto));

            var order = new Order
            {
                UserId = createOrderDto.UserId,
                ShippingAddress = new Address
                {
                    Street = createOrderDto.ShippingAddress.Street,
                    City = createOrderDto.ShippingAddress.City,
                    Zip = createOrderDto.ShippingAddress.Zip
                },
                OrderItems = createOrderDto.OrderItems.Select(oi => new OrderItem
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.ProductName,
                    ProductPrice = oi.ProductPrice,
                    Quantity = oi.Quantity,
                    VendorId = oi.VendorId,
                    VendorName = oi.VendorName,
                    FulfillmentStatus = oi.FulfillmentStatus ?? "Pending" // Provide a default if not supplied
                }).ToList()
            };

            var createdOrder = await _orderRepository.CreateOrderAsync(order);
            return MapToOrderDto(createdOrder);
        }

        // Update an existing order
        public async Task<OrderDto> UpdateOrderAsync(UpdateOrderDto updateOrderDto)
        {
            if (updateOrderDto == null)
                throw new ArgumentNullException(nameof(updateOrderDto));

            var order = await _orderRepository.GetOrderByIdAsync(updateOrderDto.Id);
            if (order == null)
                throw new KeyNotFoundException($"Order with ID {updateOrderDto.Id} not found.");

            // Update the relevant fields
            order.Status = updateOrderDto.Status;
            order.ShippingAddress = new Address
            {
                Street = updateOrderDto.ShippingAddress.Street,
                City = updateOrderDto.ShippingAddress.City,
                Zip = updateOrderDto.ShippingAddress.Zip
            };
            order.OrderItems = updateOrderDto.OrderItems.Select(oi => new OrderItem
            {
                ProductId = oi.ProductId,
                ProductName = oi.ProductName,
                ProductPrice = oi.ProductPrice,
                Quantity = oi.Quantity,
                VendorId = oi.VendorId,
                VendorName = oi.VendorName,
                FulfillmentStatus = oi.FulfillmentStatus ?? "Pending" // Ensure this has a value
            }).ToList();

            var updatedOrder = await _orderRepository.UpdateOrderAsync(order);
            return MapToOrderDto(updatedOrder);
        }

        // Delete an order by ID
        public async Task DeleteOrderAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Order ID cannot be null or empty.", nameof(id));

            await _orderRepository.DeleteOrderAsync(id);
        }

        // Map Order entity to OrderDto for reuse
        private OrderDto MapToOrderDto(Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                CreatedAt = order.CreatedAt,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                ShippingAddress = new AddressDto
                {
                    Street = order.ShippingAddress.Street,
                    City = order.ShippingAddress.City,
                    Zip = order.ShippingAddress.Zip
                },
                OrderItems = order.OrderItems.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.ProductName,
                    ProductPrice = oi.ProductPrice,
                    Quantity = oi.Quantity,
                    VendorId = oi.VendorId,
                    VendorName = oi.VendorName,
                    FulfillmentStatus = oi.FulfillmentStatus
                }).ToList()
            };
        }
    }
}
