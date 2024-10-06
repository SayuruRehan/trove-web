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
        [RegularExpression(@"^\+?[1-9]\d{1, 14}$", ErrorMessage ="Invalid phone number format")]
        public int VendorPhone {get; set;}

        [Required(ErrorMessage = "Address is required")]
        [StringLength(200, ErrorMessage ="Address cannot exceed 200 characters")]
        public string VendorAddress {get; set;}

        [Required(ErrorMessage = "City name is required")]
        [StringLength(100, ErrorMessage ="City cannot exceed 100 characters")]
        public string VendorCity {get; set;}
    }
}