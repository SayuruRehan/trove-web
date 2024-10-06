using backend.DTOs;
using backend.Interfaces;
using backend.Models;


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
            return orders.Select(order => new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                OrderId = order.OrderId,  // Direct mapping
                UserName = order.UserName,  // Direct mapping
                CreatedAt = order.CreatedAt,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                ShippingAddress = order.ShippingAddress,
                MobileNumber = order.MobileNumber,
                OrderItemIds = order.OrderItemIds  // Direct mapping
            });
        }

        // Get order by ID
        public async Task<OrderDto> GetOrderByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return null;

            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null)
                return null;

            return new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                OrderId = order.OrderId,  // Direct mapping
                UserName = order.UserName,  // Direct mapping
                CreatedAt = order.CreatedAt,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                ShippingAddress = order.ShippingAddress,
                MobileNumber = order.MobileNumber,
                OrderItemIds = order.OrderItemIds  // Direct mapping
            };
        }

        // Create a new order
        public async Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            if (createOrderDto == null)
                throw new ArgumentNullException(nameof(createOrderDto));

            // Create OrderItems list from the DTO
            var orderItems = createOrderDto.OrderItems.Select(itemDto => new OrderItem
            {
                ProductId = itemDto.ProductId,
                ProductName = itemDto.ProductName,
                ProductPrice = itemDto.ProductPrice,
                Quantity = itemDto.Quantity,
                VendorId = itemDto.VendorId,
                VendorName = itemDto.VendorName,
                ShippingAddress = itemDto.ShippingAddress,
                FulfillmentStatus = itemDto.FulfillmentStatus ?? FulfillmentStatusEnum.Pending.ToString(),
                Amount = itemDto.Amount
            }).ToList();

            // Insert the OrderItems and get their MongoDB ObjectIds
            var orderItemIds = await _orderRepository.AddOrderItemsAsync(orderItems);

            // Create the Order object with the OrderItemIds
            var order = new Order
            {
                UserId = createOrderDto.UserId,
                OrderId = createOrderDto.OrderId,  // Direct mapping
                UserName = createOrderDto.UserName,  // Direct mapping
                TotalAmount = createOrderDto.TotalAmount,
                ShippingAddress = createOrderDto.ShippingAddress,
                MobileNumber = createOrderDto.MobileNumber,
                Status = OrderStatus.Pending.ToString(),
                OrderItemIds = orderItemIds,  // Direct mapping to the list of inserted OrderItemIds
            };

            // Insert the order into the database
            var createdOrder = await _orderRepository.CreateOrderAsync(order);

            // Return the created order as a DTO
            return new OrderDto
            {
                Id = createdOrder.Id,
                UserId = createdOrder.UserId,
                OrderId = createdOrder.OrderId,  // Direct mapping
                UserName = createdOrder.UserName,  // Direct mapping
                CreatedAt = createdOrder.CreatedAt,
                Status = createdOrder.Status,
                TotalAmount = createdOrder.TotalAmount,
                ShippingAddress = createdOrder.ShippingAddress,
                MobileNumber = createdOrder.MobileNumber,
                OrderItemIds = createdOrder.OrderItemIds  // Direct mapping
            };
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
            order.UserName = updateOrderDto.UserName;  // Direct mapping
            order.ShippingAddress = updateOrderDto.ShippingAddress;
            order.MobileNumber = updateOrderDto.MobileNumber;



            var updatedOrder = await _orderRepository.UpdateOrderAsync(order);

            // Return the updated order as a DTO
            return new OrderDto
            {
                Id = updatedOrder.Id,
                UserId = order.UserId,
                OrderId = order.OrderId,  // Direct mapping
                UserName = updatedOrder.UserName,  // Direct mapping
                CreatedAt = order.CreatedAt,
                Status = updatedOrder.Status,
                TotalAmount = order.TotalAmount,
                ShippingAddress = updatedOrder.ShippingAddress,
                MobileNumber = updatedOrder.MobileNumber,
                OrderItemIds = order.OrderItemIds,  // Direct mapping
            };
        }


        // Delete an order by ID
        public async Task DeleteOrderAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Order ID cannot be null or empty.", nameof(id));

            await _orderRepository.DeleteOrderAsync(id);
        }
        public async Task<IEnumerable<OrderItemDto>> GetSubOrdersByVendorIdAsync(string vendorId)
        {
            return await _orderRepository.GetSubOrdersByVendorIdAsync(vendorId);
        }

        public async Task<OrderItemDto> UpdateOrderItemAsync(UpdateOrderItemDto updateOrderItemDto)
        {
            var updatedOrderItem = await _orderRepository.UpdateOrderItemAsync(updateOrderItemDto);

            return new OrderItemDto
            {
                Id = updatedOrderItem.Id,
                ProductId = updatedOrderItem.ProductId,
                ProductName = updatedOrderItem.ProductName,
                ProductPrice = updatedOrderItem.ProductPrice,
                Quantity = updatedOrderItem.Quantity,
                VendorId = updatedOrderItem.VendorId,
                CreatedAt = updatedOrderItem.CreatedAt,
                VendorName = updatedOrderItem.VendorName,
                FulfillmentStatus = updatedOrderItem.FulfillmentStatus,
                Amount = updatedOrderItem.Amount
            };
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersWithItemsAsync()
        {
            return await _orderRepository.GetAllOrdersWithItemsAsync();
        }


    }
}
