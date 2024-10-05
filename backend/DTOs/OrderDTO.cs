using System;
using System.Collections.Generic;

namespace backend.DTOs
{
    public class OrderDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public AddressDto ShippingAddress { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }

    public class CreateOrderDto
    {
        public string UserId { get; set; }
        public AddressDto ShippingAddress { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }

    public class UpdateOrderDto
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public AddressDto ShippingAddress { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }

    public class AddressDto
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
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
