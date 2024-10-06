using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backend.Models
{
    public class CustomerFeedback
    {
        [BsonId] 
        [BsonElement ("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string? FeedbackId {get; set;}
        public required string CustomerId {get; set;}
        public required string CustomerFeedbackText {get; set;}
        public required int Rating {get; set;}

    }
}