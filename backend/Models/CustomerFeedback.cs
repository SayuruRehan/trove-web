// IT21470004 - BOPITIYA S. R. - Customer Feedback Model

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace backend.Models
{
    [CollectionName("Feedbacks")]
    public class CustomerFeedback
    {
        [BsonId] 
        [BsonElement ("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string FeedbackId {get; set;}

        [BsonElement("userId")]  // Reference to user
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }  // Link to the MongoUser model

        [BsonElement("vendorId")]  // Reference to user
        [BsonRepresentation(BsonType.ObjectId)]
        public string VendorId { get; set; }  

        [BsonElement("FirstName")]
        public string FirstName {get; set;}

        [BsonElement("LastName")]
        public string LastName {get; set;}

        public string CustomerFeedbackText {get; set;}

        public int Rating {get; set;}

    }
}