// IT21470004 - BOPITIYA S. R. - Product Model

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace backend.Models
{
    [CollectionName("Products")] // Ensure the collection name matches
    public class Product
    {
        [BsonId]
        [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } // Use string for ObjectId compatibility

        [BsonElement("name")]
        public string ProductName { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("imageUrl")]
        public string ImageUrl { get; set; } // Store the Cloudinary image URL

        [BsonElement("price")]
        public decimal ProductPrice { get; set; }

        [BsonElement("stock")]
        public int Stock { get; set; }


        [BsonElement("vendorId")]
        public string VendorId { get; set; }
        public string VendorName { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
