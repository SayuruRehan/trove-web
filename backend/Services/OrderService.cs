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

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllOrdersAsync();
            return orders.Select(o => new OrderDto
            {
                Id = o.Id,
                UserId = o.UserId,
                CreatedAt = o.CreatedAt,
                Status = o.Status,
                TotalAmount = o.TotalAmount,
                ShippingAddress = new AddressDto
                {
                    Street = o.ShippingAddress.Street,
                    City = o.ShippingAddress.City,
                    Zip = o.ShippingAddress.Zip
                },
                OrderItems = o.OrderItems.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.ProductName,
                    ProductPrice = oi.ProductPrice,
                    Quantity = oi.Quantity,
                    VendorId = oi.VendorId,
                    VendorName = oi.VendorName,
                    FulfillmentStatus = oi.FulfillmentStatus
                }).ToList()
            });
        }

        public async Task<OrderDto> GetOrderByIdAsync(string id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null) return null;

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

        public async Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto)
        {
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
                    FulfillmentStatus = oi.FulfillmentStatus
                }).ToList()
            };

            var createdOrder = await _orderRepository.CreateOrderAsync(order);
            return new OrderDto
            {
                Id = createdOrder.Id,
                UserId = createdOrder.UserId,
                CreatedAt = createdOrder.CreatedAt,
                Status = createdOrder.Status,
                TotalAmount = createdOrder.TotalAmount,
                ShippingAddress = new AddressDto
                {
                    Street = createdOrder.ShippingAddress.Street,
                    City = createdOrder.ShippingAddress.City,
                    Zip = createdOrder.ShippingAddress.Zip
                },
                OrderItems = createdOrder.OrderItems.Select(oi => new OrderItemDto
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

        public async Task<OrderDto> UpdateOrderAsync(UpdateOrderDto updateOrderDto)
        {
            var order = new Order
            {
                Id = updateOrderDto.Id,
                Status = updateOrderDto.Status,
                ShippingAddress = new Address
                {
                    Street = updateOrderDto.ShippingAddress.Street,
                    City = updateOrderDto.ShippingAddress.City,
                    Zip = updateOrderDto.ShippingAddress.Zip
                },
                OrderItems = updateOrderDto.OrderItems.Select(oi => new OrderItem
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

            var updatedOrder = await _orderRepository.UpdateOrderAsync(order);
            return new OrderDto
            {
                Id = updatedOrder.Id,
                UserId = updatedOrder.UserId,
                CreatedAt = updatedOrder.CreatedAt,
                Status = updatedOrder.Status,
                TotalAmount = updatedOrder.TotalAmount,
                ShippingAddress = new AddressDto
                {
                    Street = updatedOrder.ShippingAddress.Street,
                    City = updatedOrder.ShippingAddress.City,
                    Zip = updatedOrder.ShippingAddress.Zip
                },
                OrderItems = updatedOrder.OrderItems.Select(oi => new OrderItemDto
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

        public async Task DeleteOrderAsync(string id)
        {
            await _orderRepository.DeleteOrderAsync(id);
        }
    }
}
