using System.ComponentModel.DataAnnotations;
using backend.Models;

namespace backend.DTOs
{
    // CreateVendorDTO class for vendor management
    public class CreateVendorDTO 
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage ="Name cannot exceed 100 characters")]
        public string VendorName {get; set;}

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage ="Invalid email format")]
        public string VendorEmail {get; set;}

        [Required(ErrorMessage = "Pohne number is required")]
        public string VendorPhone {get; set;}

        [Required(ErrorMessage = "Address is required")]
        [StringLength(200, ErrorMessage ="Address cannot exceed 200 characters")]
        public string VendorAddress {get; set;}

        [Required(ErrorMessage = "City name is required")]
        [StringLength(100, ErrorMessage ="City cannot exceed 100 characters")]
        public string VendorCity {get; set;}

        public string HashedPassword {get; set;}

        public bool IsActive {get; set;} = false;

        public List<string> Products {get; set;} = new List<string>();

        public List<CustomerFeedback> Feedbacks {get; set;} = new List<CustomerFeedback>();
    }
}