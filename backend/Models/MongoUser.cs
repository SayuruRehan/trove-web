using AspNetCore.Identity.MongoDbCore.Models;
using Microsoft.AspNetCore.Identity;
using MongoDbGenericRepository.Attributes;
using System;

namespace backend.Models
{
    [CollectionName("Users")]
    public class User : MongoIdentityUser<Guid>
    {

        [PersonalData]
        public string Firstname {get; set;}

        [PersonalData]
        public string Lastname {get; set;}

        [PersonalData]
        public string Phone {get; set;}

    }
}
