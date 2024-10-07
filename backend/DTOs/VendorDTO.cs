using backend.Models;

namespace backend.DTOs
{
    // VendorDTO class for vendor management
    public class VendorDTO
    {
        public string Id {get; set;}

        public string VendorName {get; set;}

        public string VendorEmail {get; set;}

        public string VendorPhone {get; set;}

        public string VendorAddress {get; set;}

        public string VendorCity {get; set;}

        public bool IsActive {get; set;} = false;

        // public string HashedPassword {get; set;}
        
        public List<string> Products {get; set;} = new List<string>();

        public List<CustomerFeedback> Feedbacks {get; set;} = new List<CustomerFeedback>();
    }
}