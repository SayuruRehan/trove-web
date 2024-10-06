using System;
using AspNetCore.Identity.MongoDbCore.Models;
using Microsoft.AspNetCore.Identity;
using MongoDbGenericRepository.Attributes;
namespace backend.Models{
    [CollectionName("Vendors")]
    public class Vendor : MongoIdentityUser<Guid>{
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