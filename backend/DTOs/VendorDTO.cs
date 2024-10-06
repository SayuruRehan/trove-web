using backend.Models;

namespace backend.DTOs
{
    public class VendorDTO
    {
        public string Id {get; set;}

        public required string VendorName {get; set;}

        public required string VendorEmail {get; set;}

        public required string VendorPhone {get; set;}

        public required string VendorAddress {get; set;}

        public required string VendorCity {get; set;}

        public bool IsActive {get; set;} = false;

        public string HashedPassword {get; set;}
        
        public List<string> Products {get; set;} = new List<string>();

        public List<CustomerFeedback> Feedbacks {get; set;} = new List<CustomerFeedback>();
    }
}