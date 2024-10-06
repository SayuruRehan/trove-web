using AspNetCore.Identity.MongoDbCore.Models;
using Microsoft.AspNetCore.Identity;
using MongoDbGenericRepository.Attributes;
using MongoDB.Bson.Serialization.Attributes; // For BsonElement
using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson; // For Required and EnumDataType attributes

namespace backend.Models
{
    [CollectionName("Users")]
    public class User : MongoIdentityUser<ObjectId>
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public override ObjectId Id { get; set; }  // Override Id to use ObjectId

        [PersonalData]
        public string Firstname { get; set; }

        [PersonalData]
        public string Lastname { get; set; }

        [PersonalData]
        public string Phone { get; set; }

        public string Role { get; set; }

        [BsonElement("status")]
        [EnumDataType(typeof(UserStatus))]
        public UserStatus Status { get; set; } = UserStatus.Deactive;
    }

    public enum UserStatus
    {
        Active = 1,
        Deactive = 0
    }
}
