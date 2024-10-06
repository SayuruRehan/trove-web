using System;

namespace backend.DTOs
{
    public class OrderItemDto
    {
        public string Id { get; set; }  // Added Id property
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; }
        public string VendorId { get; set; }
        public string VendorName { get; set; }
        public string FulfillmentStatus { get; set; }
        public decimal Amount { get; set; }  // Changed from double? to decimal
        public string ShippingAddress { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class UpdateOrderItemDto
    {
        public string Id { get; set; }  // Added Id property
        public string ProductId { get; set; }
        public string ProductName { get; set; }  // Add ProductName
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; }
        public string VendorId { get; set; }
        public string VendorName { get; set; }
        public string FulfillmentStatus { get; set; }
        public decimal Amount { get; set; }
        public string ShippingAddress { get; set; }
    }

    // You might want to add this DTO for creating new OrderItems
    public class CreateOrderItemDto
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; }
        public string VendorId { get; set; }
        public string VendorName { get; set; }
        public string FulfillmentStatus { get; set; }
        public decimal Amount { get; set; }
        public string ShippingAddress { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}