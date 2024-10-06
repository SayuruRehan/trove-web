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
                    Stock = product.Stock
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
                Stock = product.Stock
            };
        }

        // Create a new product
        public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto)
        {
            var uploadResult = await _cloudinaryService.UploadImageAsync(createProductDto.Image);

            if (uploadResult.Error != null)
            {
                throw new Exception(uploadResult.Error.Message);
            }

            var product = new Product
            {
                ProductName = createProductDto.ProductName,
                Description = createProductDto.Description,
                ProductPrice = createProductDto.ProductPrice,
                Stock = createProductDto.Stock,
                ImageUrl = uploadResult.SecureUrl.ToString(),
                VendorId = createProductDto.VendorId
            };

            var createdProduct = await _productRepository.CreateProductAsync(product);

            return new ProductDto
            {
                Id = createdProduct.Id,
                ProductName = createdProduct.ProductName,
                Description = createdProduct.Description,
                ProductPrice = createdProduct.ProductPrice,
                Stock = createdProduct.Stock,
                ImageUrl = createdProduct.ImageUrl,
                VendorId = createdProduct.VendorId
            };
        }

        // Update an existing product
        public async Task<ProductDto> UpdateProductAsync(UpdateProductDto updateProductDto)
        {
            if (string.IsNullOrEmpty(updateProductDto.Id))
                throw new ArgumentException("Invalid product ID.");

            var product = new Product
            {
                Id = updateProductDto.Id,
                ProductName = updateProductDto.ProductName,
                Description = updateProductDto.Description,
                ProductPrice = updateProductDto.ProductPrice,
                Stock = updateProductDto.Stock
            };

            var updatedProduct = await _productRepository.UpdateProductAsync(product);
            return new ProductDto
            {
                Id = updatedProduct.Id,
                ProductName = updatedProduct.ProductName,
                Description = updatedProduct.Description,
                ProductPrice = updatedProduct.ProductPrice,
                Stock = updatedProduct.Stock
            };
        }

        // Delete a product by ID
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
                VendorId = product.VendorId
            });

            return productDtos;
        }
    }
}