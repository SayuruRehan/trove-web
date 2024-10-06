using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    // VendorLoginDTO class for vendor management
    public class VendorLoginDTO
    {
        [Required(ErrorMessage ="Email is required")]
        [EmailAddress]
        public string VendorEmail { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string HashedPassword { get; set; }
    }
}
