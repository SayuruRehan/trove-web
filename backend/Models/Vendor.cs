using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace backend.Models
{
    public class Vendor : User
    {

        [BsonElement("businessName")]
        public string BusinessName { get; set; }  // Additional vendor-specific field

        [BsonElement("ratings")]
        public int Ratings {get; set;}
    }

}
