using System.ComponentModel.DataAnnotations;

namespace backend.DTOs
{
    public class ProductDto
    {
        public string Id { get; set; }

        public string ProductName { get; set; }

        public string Description { get; set; }

        public decimal ProductPrice { get; set; }

        public string ImageUrl { get; set; }

        public string VendorId { get; set; }

        public int Stock { get; set; }
    }

    public class CreateProductDto
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string ProductName { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal ProductPrice { get; set; }

        [Required(ErrorMessage = "Stock is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative.")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "Vender id is required.")]
        public string VendorId { get; set; }

        public IFormFile? Image { get; set; }
    }

    public class UpdateProductDto
    {

        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string ProductName { get; set; } // This should be optional for partial updates

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; } // Optional for partial updates

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        public decimal ProductPrice { get; set; } // Required for update

        [Required(ErrorMessage = "Stock is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative.")]
        public int Stock { get; set; } // Required for update

        public IFormFile? Image { get; set; } // Optional: only provide if there's a new image to upload
    }
};