// IT21470004 - BOPITIYA S. R. - Vendor model 

using MongoDbGenericRepository.Attributes;

namespace backend.Models
{
    // Vendor model for manage mongoDB collection
    [CollectionName("Vendors")]
    public class Vendor : User
    {
        public decimal Rank { get; set; } = 0;
    }
}