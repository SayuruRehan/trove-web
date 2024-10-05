using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace backend.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("userId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }  // Reference to the User

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("status")]
        public string Status { get; set; } = "Pending"; // Order status: Pending, Partially Fulfilled, Fulfilled

        [BsonElement("totalAmount")]
        public decimal TotalAmount { get; set; }

        [BsonElement("shippingAddress")]
        public Address ShippingAddress { get; set; }

        [BsonElement("orderItems")]
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }

    public class Address
    {
        [BsonElement("street")]
        public string Street { get; set; }

        [BsonElement("city")]
        public string City { get; set; }

        [BsonElement("zip")]
        public string Zip { get; set; }
    }

    public class OrderItem
    {
        [BsonElement("productId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ProductId { get; set; }  // Reference to Product

        [BsonElement("productName")]
        public string ProductName { get; set; }  // Denormalized for fast retrieval

        [BsonElement("productPrice")]
        public decimal ProductPrice { get; set; }  // Denormalized

        [BsonElement("quantity")]
        public int Quantity { get; set; }

        [BsonElement("vendorId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string VendorId { get; set; }  // Reference to Vendor

        [BsonElement("vendorName")]
        public string VendorName { get; set; }  // Denormalized for fast retrieval

        [BsonElement("fulfillmentStatus")]
        public string FulfillmentStatus { get; set; } = "Pending";  // Track the fulfillment status of the item
    }
}
