using System;
using System.Collections.Generic;

namespace backend.DTOs
{
    public class OrderDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string orderId { get; set;}
        public DateTime CreatedAt { get; set; }
        public string status { get; set; }
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; }
        public string MobileNumber { get; set; } 
        public string userName { get; set; } 
        public List<OrderItemDto> OrderItems { get; set; }
    }

    public class CreateOrderDto
    {
        public string UserId { get; set; }
        public string orderId { get; set;}
        public string ShippingAddress { get; set; }
        public decimal TotalAmount { get; set; }
        public string MobileNumber { get; set; }
        public string userName { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }

    public class UpdateOrderDto
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public string ShippingAddress { get; set; }
        public string MobileNumber { get; set; }
        public string userName { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }

    public class OrderItemDto
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; }
        public string VendorId { get; set; }
        public string VendorName { get; set; }
        public string FulfillmentStatus { get; set; }
    }
}
