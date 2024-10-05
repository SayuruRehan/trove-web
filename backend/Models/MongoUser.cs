using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;
using System;

namespace backend.Models
{
    [CollectionName("Users")]
    public class User : MongoIdentityUser<Guid>
    {
    }
}
