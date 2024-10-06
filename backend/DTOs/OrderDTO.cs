using System;
using System.Collections.Generic;

namespace backend.DTOs
{
    public class OrderDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string OrderId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; }
        public string MobileNumber { get; set; }
        public string UserName { get; set; }
        public List<string> OrderItemIds { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }

    public class CreateOrderDto
    {
        public string UserId { get; set; }
        public string OrderId { get; set; }
        public string ShippingAddress { get; set; }
        public decimal TotalAmount { get; set; }
        public string MobileNumber { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<CreateOrderItemDto> OrderItems { get; set; }  // Changed from List<string> OrderItemsIds
    }

    public class UpdateOrderDto
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public string ShippingAddress { get; set; }
        public string MobileNumber { get; set; }
        public string UserName { get; set; }
        // Changed from List<string> OrderItemsIds
    }
}