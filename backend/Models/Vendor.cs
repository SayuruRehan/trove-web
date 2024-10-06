using System;
using AspNetCore.Identity.MongoDbCore.Models;
using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace backend.Models{

    [CollectionName("Vendors")]
    public class Vendor : MongoIdentityUser<Guid>{

        // Explicitly say this is the primary key for the MongoDB document
        [BsonId] 
        // Tells in MongoDb treat as ObjectId and in application treat as string
        [BsonElement ("_id"), BsonRepresentation(BsonType.ObjectId)]

        [PersonalData]
        public string? Id {get; set;}

        [PersonalData]
        public string VendorName {get; set;}

        [PersonalData]
        public string VendorEmail {get; set;}

        [PersonalData]
        public int VendorPhone {get; set;}

        public string VendorAddress {get; set;}

        public string VendorCity {get; set;}
    }
}

