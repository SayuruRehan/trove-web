using backend.DTOs;
using backend.Interfaces;
using backend.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backend.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
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
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
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
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock
            };
        }

        // Create a new product
        public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto)
        {
            var product = new Product
            {
                Name = createProductDto.Name,
                Description = createProductDto.Description,
                Price = createProductDto.Price,
                Stock = createProductDto.Stock
            };

            var createdProduct = await _productRepository.CreateProductAsync(product);
            return new ProductDto
            {
                Id = createdProduct.Id,
                Name = createdProduct.Name,
                Description = createdProduct.Description,
                Price = createdProduct.Price,
                Stock = createdProduct.Stock
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
                Name = updateProductDto.Name,
                Description = updateProductDto.Description,
                Price = updateProductDto.Price,
                Stock = updateProductDto.Stock
            };

            var updatedProduct = await _productRepository.UpdateProductAsync(product);
            return new ProductDto
            {
                Id = updatedProduct.Id,
                Name = updatedProduct.Name,
                Description = updatedProduct.Description,
                Price = updatedProduct.Price,
                Stock = updatedProduct.Stock
            };
        }

        // Delete a product by ID
        public Task DeleteProductAsync(string id) => _productRepository.DeleteProductAsync(id);
    }
}
