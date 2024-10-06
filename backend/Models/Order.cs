using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required]
        [BsonElement("orderId")]
        public string orderId { get; set;}
        
        [Required]
        [BsonElement("userId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }  // Reference to the User

        [BsonElement("createdAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [BsonElement("mobileNumber")]
        [Phone(ErrorMessage = "Invalid mobile number format.")]
        public string MobileNumber { get; set;} 

        [Required]
        [BsonElement("userName")]
        public string userName { get; set;} 
        
        [Required]
        [BsonElement("status")]
        [EnumDataType(typeof(OrderStatus))]
        public string Status { get; set; } = OrderStatus.Pending.ToString(); // Enforce specific status types

        [Required]
        [BsonElement("totalAmount")]
        [Range(0, Double.MaxValue, ErrorMessage = "Total amount must be a positive value.")]
        public decimal TotalAmount { get; set; }

        [Required]
        [BsonElement("shippingAddress")]
        public string ShippingAddress { get; set; }

        [Required]
        [BsonElement("orderItems")]
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }

    public class OrderItem
    {
        [Required]
        [BsonElement("productId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductId { get; set; }  // Reference to Product

        [Required]
        [BsonElement("productName")]
        public string ProductName { get; set; }  // Denormalized for fast retrieval

        [Required]
        [BsonElement("productPrice")]
        [Range(0, Double.MaxValue, ErrorMessage = "Product price must be a positive value.")]
        public decimal ProductPrice { get; set; }  // Denormalized

        [Required]
        [BsonElement("quantity")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Required]
        [BsonElement("vendorId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string VendorId { get; set; }  // Reference to Vendor

        [Required]
        [BsonElement("vendorName")]
        public string VendorName { get; set; }  // Denormalized for fast retrieval

        [Required]
        [BsonElement("fulfillmentStatus")]
        [EnumDataType(typeof(FulfillmentStatusEnum))]
        public string FulfillmentStatus { get; set; } = FulfillmentStatusEnum.Pending.ToString();  // Track fulfillment status

        [Required]
        [BsonElement("status")]
        [EnumDataType(typeof(OrderStatus))]
        public string Status { get; set; } = OrderStatus.Pending.ToString();
    }

    public enum OrderStatus
    {
        Pending,
        PartiallyFulfilled,
        Fulfilled
    }

    public enum FulfillmentStatusEnum
    {
        Pending,
        Fulfilled,
        Shipped,
        Cancelled
    }
}
