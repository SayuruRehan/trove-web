using backend.DTOs;
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
        private readonly IMongoCollection<OrderItem> _orderItems;

        public OrderRepository(IMongoDatabase database)
        {
            _orders = database.GetCollection<Order>("Orders");
            _orderItems = database.GetCollection<OrderItem>("orderItems");
        }

        // Get all orders
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orders.Find(FilterDefinition<Order>.Empty).ToListAsync();
        }

        // Get order by ID
        public async Task<Order> GetOrderByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;

            var filter = Builders<Order>.Filter.Eq(o => o.Id, id);
            return await _orders.Find(filter).FirstOrDefaultAsync();
        }

        // Create a new order
        public async Task<Order> CreateOrderAsync(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            await _orders.InsertOneAsync(order);
            return order;
        }

        // Update an existing order
        public async Task<Order> UpdateOrderAsync(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            var filter = Builders<Order>.Filter.Eq(o => o.Id, order.Id);
            var result = await _orders.ReplaceOneAsync(filter, order);

            if (result.ModifiedCount == 0)
                throw new KeyNotFoundException($"Order with ID {order.Id} not found.");

            return order;
        }

        // Delete an order by ID
        public async Task DeleteOrderAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Order ID cannot be null or empty.", nameof(id));

            var filter = Builders<Order>.Filter.Eq(o => o.Id, id);
            var result = await _orders.DeleteOneAsync(filter);

            if (result.DeletedCount == 0)
                throw new KeyNotFoundException($"Order with ID {id} not found.");
        }

        public async Task<List<string>> AddOrderItemsAsync(List<OrderItem> orderItems)
        {
            if (orderItems == null || orderItems.Count == 0)
            {
                throw new ArgumentException("Order items cannot be null or empty.");
            }

            // Insert multiple order items at once
            await _orderItems.InsertManyAsync(orderItems);

            // Return the inserted ObjectIds as strings
            return orderItems.Select(item => item.Id.ToString()).ToList();
        }

        public async Task<IEnumerable<OrderItemDto>> GetSubOrdersByVendorIdAsync(string vendorId)
        {
            // Get all orders from the database
            var orders = await _orders.Find(FilterDefinition<Order>.Empty).ToListAsync();

            // Extract all order item IDs from all orders
            var orderItemIds = orders
                .SelectMany(order => order.OrderItemIds) // Assuming `OrderItemIds` is the list of order item IDs
                .ToList();

            // Retrieve the actual order items from the database using their IDs
            var filter = Builders<OrderItem>.Filter.In(orderItem => orderItem.Id, orderItemIds);
            var orderItems = await _orderItems.Find(filter).ToListAsync(); // Assuming `_orderItems` is your order item collection

            // Filter order items by the given vendorId
            var subOrders = orderItems
                .Where(orderItem => orderItem.VendorId == vendorId)
                .Select(orderItem => new OrderItemDto
                {
                    Id = orderItem.Id,
                    ProductId = orderItem.ProductId,
                    ProductName = orderItem.ProductName,
                    ProductPrice = orderItem.ProductPrice,
                    Quantity = orderItem.Quantity,
                    VendorId = orderItem.VendorId,
                    CreatedAt = orderItem.CreatedAt,
                    VendorName = orderItem.VendorName,
                    FulfillmentStatus = orderItem.FulfillmentStatus,
                    ShippingAddress = orderItem.ShippingAddress,
                    Amount = orderItem.Amount
                })
                .ToList();

            return subOrders;
        }

        public async Task<OrderItem> UpdateOrderItemAsync(UpdateOrderItemDto updateOrderItemDto)
        {
            if (updateOrderItemDto == null)
                throw new ArgumentNullException(nameof(updateOrderItemDto));

            var filter = Builders<OrderItem>.Filter.Eq(item => item.Id, updateOrderItemDto.Id);
            var orderItem = await _orderItems.Find(filter).FirstOrDefaultAsync();

            if (orderItem == null)
                throw new KeyNotFoundException($"OrderItem with ID {updateOrderItemDto.Id} not found.");

            // Update the order item fields
            orderItem.ProductId = updateOrderItemDto.ProductId;
            orderItem.ProductName = updateOrderItemDto.ProductName;
            orderItem.ProductPrice = updateOrderItemDto.ProductPrice;
            orderItem.Quantity = updateOrderItemDto.Quantity;
            orderItem.VendorId = updateOrderItemDto.VendorId;
            orderItem.VendorName = updateOrderItemDto.VendorName;
            orderItem.FulfillmentStatus = updateOrderItemDto.FulfillmentStatus;
            orderItem.Amount = updateOrderItemDto.Amount;
            orderItem.ShippingAddress = updateOrderItemDto.ShippingAddress;

            // Replace the updated order item in the collection
            await _orderItems.ReplaceOneAsync(filter, orderItem);
            return orderItem;
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersWithItemsAsync()
        {
            // Get all orders
            var orders = await _orders.Find(FilterDefinition<Order>.Empty).ToListAsync();

            // Extract all order item IDs from all orders
            var orderItemIds = orders.SelectMany(order => order.OrderItemIds).ToList();

            // Retrieve the actual order items from the database using their IDs
            var filter = Builders<OrderItem>.Filter.In(orderItem => orderItem.Id, orderItemIds);
            var orderItems = await _orderItems.Find(filter).ToListAsync();

            // Map the orders and their corresponding order items to OrderDto
            var orderDtos = orders.Select(order => new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                CreatedAt = order.CreatedAt,
                Status = order.Status,
                OrderId = order.OrderId,
                TotalAmount = order.TotalAmount,
                UserName = order.UserName,
                MobileNumber = order.MobileNumber,
                ShippingAddress = order.ShippingAddress,
                OrderItemIds = order.OrderItemIds,
                OrderItems = order.OrderItemIds
                    .Select(itemId => orderItems.FirstOrDefault(item => item.Id == itemId))
                    .Where(item => item != null)
                    .Select(item => new OrderItemDto
                    {
                        Id = item.Id,
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        ProductPrice = item.ProductPrice,
                        Quantity = item.Quantity,
                        VendorId = item.VendorId,
                        CreatedAt = item.CreatedAt,
                        VendorName = item.VendorName,
                        FulfillmentStatus = item.FulfillmentStatus,
                        ShippingAddress = item.ShippingAddress,
                        Amount = item.Amount
                    })
                    .ToList()
            }).ToList();

            return orderDtos;
        }


    }
}
