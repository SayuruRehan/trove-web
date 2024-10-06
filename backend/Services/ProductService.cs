using backend.DTOs;
using backend.Interfaces;
using backend.Models;


namespace backend.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly CloudinaryService _cloudinaryService;

        public ProductService(IProductRepository productRepository, CloudinaryService cloudinaryService)
        {
            _productRepository = productRepository;
            _cloudinaryService = cloudinaryService;
        }

        // Retrieve all products
        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllProductsAsync();
            // Map Product to ProductDto
            var productDtos = new List<ProductDto>();
            foreach (var product in products)
            {
                productDtos.Add(new ProductDto
                {
                    Id = product.Id,
                    ProductName = product.ProductName,
                    Description = product.Description,
                    ProductPrice = product.ProductPrice,
                    Stock = product.Stock,
                    ImageUrl = product.ImageUrl,
                    VendorId = product.VendorId,
                    VendorName = product.VendorName
                });
            }
            return productDtos;
        }

        // Retrieve a product by ID
        public async Task<ProductDto> GetProductByIdAsync(string id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null)
                return null; // Or throw an exception if needed

            return new ProductDto
            {
                Id = product.Id,
                ProductName = product.ProductName,
                Description = product.Description,
                ProductPrice = product.ProductPrice,
                Stock = product.Stock,
                VendorId = product.VendorId,
                ImageUrl = product.ImageUrl,
                VendorName = product.VendorName
            };
        }

        // Create a new product
        public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto)
        {
            string imageUrl = null;

            // Only attempt to upload if the image is provided
            if (createProductDto.Image != null && createProductDto.Image.Length > 0)
            {
                var uploadResult = await _cloudinaryService.UploadImageAsync(createProductDto.Image);

                // Handle potential errors from the upload
                if (uploadResult.Error != null)
                {
                    throw new Exception(uploadResult.Error.Message);
                }

                // Set the secure URL from the uploaded image
                imageUrl = uploadResult.SecureUrl.ToString();
            }

            // Create the product object
            var product = new Product
            {
                ProductName = createProductDto.ProductName,
                Description = createProductDto.Description,
                ProductPrice = createProductDto.ProductPrice,
                Stock = createProductDto.Stock,
                ImageUrl = imageUrl,  // Only set the image URL if an image was uploaded
                VendorId = createProductDto.VendorId,
                VendorName = createProductDto.VendorName
            };

            // Save the product in the repository
            var createdProduct = await _productRepository.CreateProductAsync(product);

            // Return the created product as a DTO
            return new ProductDto
            {
                Id = createdProduct.Id,
                ProductName = createdProduct.ProductName,
                Description = createdProduct.Description,
                ProductPrice = createdProduct.ProductPrice,
                Stock = createdProduct.Stock,
                ImageUrl = createdProduct.ImageUrl,  // This will be null if no image was uploaded
                VendorId = createdProduct.VendorId,
                VendorName = createdProduct.VendorName
            };
        }

        // Delete a product by ID
        public async Task<ProductDto> UpdateProductAsync(string id, UpdateProductDto updateProductDto)
        {
            // Check if the product exists
            var existingProduct = await _productRepository.GetProductByIdAsync(id);
            if (existingProduct == null)
            {
                throw new KeyNotFoundException("Product not found.");
            }


            string imageUrl = existingProduct.ImageUrl;

            if (updateProductDto.Image != null)
            {
                // If there's an existing image URL, remove it from Cloudinary (or your image hosting service)
                if (!string.IsNullOrEmpty(existingProduct.ImageUrl))
                {
                    var imagePublicId = GetImagePublicId(existingProduct.ImageUrl);  // Extract the public ID from the existing URL
                    var deleteResult = await _cloudinaryService.DeleteImageAsync(imagePublicId);  // Delete the old image
                    if (deleteResult.Error != null)
                    {
                        throw new Exception("Error deleting old image: " + deleteResult.Error.Message);
                    }
                }

                // Upload the new image to Cloudinary
                var uploadResult = await _cloudinaryService.UploadImageAsync(updateProductDto.Image);
                if (uploadResult.Error != null)
                {
                    throw new Exception(uploadResult.Error.Message);
                }

                // Update the product with the new image URL
                imageUrl = uploadResult.SecureUrl.ToString();
            }


            var product = new Product
            {
                Id = existingProduct.Id,
                ProductName = updateProductDto.ProductName ?? existingProduct.ProductName,
                Description = updateProductDto.Description ?? existingProduct.Description,
                ProductPrice = updateProductDto.ProductPrice,
                Stock = updateProductDto.Stock,
                ImageUrl = imageUrl,
                VendorId = existingProduct.VendorId,
                VendorName = existingProduct.VendorName,
            };

            // Step 4: Update the product in the repository
            var updatedProduct = await _productRepository.UpdateProductAsync(product);

            // Step 5: Return the updated product DTO
            return new ProductDto
            {
                Id = updatedProduct.Id,
                ProductName = updatedProduct.ProductName,
                Description = updatedProduct.Description,
                ProductPrice = updatedProduct.ProductPrice,
                Stock = updatedProduct.Stock,
                ImageUrl = updatedProduct.ImageUrl,
                VendorId = updatedProduct.VendorId,
                VendorName = updatedProduct.VendorName
            };
        }


        public Task DeleteProductAsync(string id) => _productRepository.DeleteProductAsync(id);

        // Retrieve products by vendorId
        public async Task<IEnumerable<ProductDto>> GetProductsByVendorIdAsync(string vendorId)
        {
            var products = await _productRepository.GetProductsByVendorIdAsync(vendorId);
            var productDtos = products.Select(product => new ProductDto
            {
                Id = product.Id,
                ProductName = product.ProductName,
                Description = product.Description,
                ProductPrice = product.ProductPrice,
                Stock = product.Stock,
                ImageUrl = product.ImageUrl,
                VendorId = product.VendorId,
                VendorName = product.VendorName
            });

            return productDtos;
        }
        private static string GetImagePublicId(string imageUrl)
        {
            var uri = new Uri(imageUrl);
            var segments = uri.AbsolutePath.Split('/');
            var publicIdWithExtension = segments[segments.Length - 1];  // "your-image.jpg"
            var publicId = Path.GetFileNameWithoutExtension(publicIdWithExtension); // "your-image"
            return publicId;
        }
    }
}
