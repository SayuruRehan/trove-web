using System;
using AspNetCore.Identity.MongoDbCore.Models;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace backend.Models
{

    [CollectionName("Vendors")]
    public class Vendor
    {

        // Explicitly say this is the primary key for the MongoDB document
        [BsonId] 
        // Tells in MongoDb treat as ObjectId and in application treat as string
        [BsonElement ("_id"), BsonRepresentation(BsonType.ObjectId)]

        [PersonalData]
        public string? Id {get; set;}

        [PersonalData]
        public required string VendorName {get; set;}

        [PersonalData]
        public required string VendorEmail {get; set;}

        [PersonalData]
        public required string VendorPhone {get; set;}

        public required string VendorAddress {get; set;}

        public required string VendorCity {get; set;}

        public List<string> Products {get; set;} = new List<string>();

        [BsonElement("CustomerFeedback")]
        public List<CustomerFeedback> Feedbacks {get; set;} = new List<CustomerFeedback>();
    }
}

