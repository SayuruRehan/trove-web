using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
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

        [StringLength(300, ErrorMessage ="Feedback cannot exceed 300 characters")]
        public string CustomerFeedback {get; set;}

        [Range(1, 10, ErrorMessage="Rating must be betweeb 1 and 10")]
        public int Rating {get; set;}
    }
}