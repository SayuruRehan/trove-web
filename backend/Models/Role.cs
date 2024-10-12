// IT21470004 - BOPITIYA S. R. - Role model

using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;
using System;

namespace backend.Models
{
    [CollectionName("Roles")]
    public class Role : MongoIdentityRole<Guid>
    {
    }
}
