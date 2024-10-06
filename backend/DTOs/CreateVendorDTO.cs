using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    public class CreateVendorDTO 
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage ="Name cannot exceed 100 characters")]
        public required string VendorName {get; set;}

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage ="Invalid email format")]
        public required string VendorEmail {get; set;}

        [Required(ErrorMessage = "Pohne number is required")]
        public required string VendorPhone {get; set;}

        [Required(ErrorMessage = "Address is required")]
        [StringLength(200, ErrorMessage ="Address cannot exceed 200 characters")]
        public required string VendorAddress {get; set;}

        [Required(ErrorMessage = "City name is required")]
        [StringLength(100, ErrorMessage ="City cannot exceed 100 characters")]
        public required string VendorCity {get; set;}

        public List<string> CustomerFeedback {get; set;}= new List<string>();

        public List<int> Rating {get; set;} = new List<int>();
    }
}