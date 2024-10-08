// IT21470004 - BOPITIYA S. R. - Order Model

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    [CollectionName("orders")]
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required]
        [BsonElement("orderId")]
        public string OrderId { get; set; }  // Changed to PascalCase

        [Required]
        [BsonElement("userId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

        [BsonElement("createdAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [BsonElement("mobileNumber")]
        [Phone(ErrorMessage = "Invalid mobile number format.")]
        public string MobileNumber { get; set; }

        [Required]
        [BsonElement("userName")]
        public string UserName { get; set; }  // Changed to PascalCase

        [Required]
        [BsonElement("status")]
        [EnumDataType(typeof(OrderStatus))]
        public string Status { get; set; } = OrderStatus.Pending.ToString();

        [Required]
        [BsonElement("totalAmount")]
        [Range(0, Double.MaxValue, ErrorMessage = "Total amount must be a positive value.")]
        public decimal TotalAmount { get; set; }

        [Required]
        [BsonElement("shippingAddress")]
        public string ShippingAddress { get; set; }

        [Required]
        [BsonElement("orderItems")]
        public List<string> OrderItemIds { get; set; }
    }

    // Added OrderItem class
    [CollectionName("orderItems")]
    public class OrderItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }  // Added Id property

        [Required]
        [BsonElement("productId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductId { get; set; }

        [Required]
        [BsonElement("productName")]
        public string ProductName { get; set; }

        [Required]
        [BsonElement("productPrice")]
        [Range(0, Double.MaxValue, ErrorMessage = "Product price must be a positive value.")]
        public decimal ProductPrice { get; set; }

        [Required]
        [BsonElement("quantity")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Required]
        [BsonElement("vendorId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string VendorId { get; set; }

        [Required]
        [BsonElement("vendorName")]
        public string VendorName { get; set; }

        [Required]
        [BsonElement("fulfillmentStatus")]
        [EnumDataType(typeof(FulfillmentStatusEnum))]
        public string FulfillmentStatus { get; set; } = FulfillmentStatusEnum.Pending.ToString();

        [BsonElement("amount")]
        public decimal Amount { get; set; }  // Changed from double? to decimal for consistency
        
        [Required]
        [BsonElement("shippingAddress")]
        public string ShippingAddress { get; set; }

        [BsonElement("createdAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum OrderStatus
    {
        Pending,
        PartiallyDelivered,
        Cancelled,
        Delivered
    }

    public enum FulfillmentStatusEnum
    {
        Pending,
        Delivered,
        PartiallyDelivered,
        Shipped,
    }
}