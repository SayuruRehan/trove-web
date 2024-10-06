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

        public List<string> CustomerFeedback {get; set;} = new List<string>();

        public List<int> Rating {get; set;} = new List<int>();
    }
}