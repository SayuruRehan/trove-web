using backend.DTOs;
using backend.Models;
using Microsoft.AspNetCore.Identity;

namespace backend.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(string id);

        Task<Product> UpdateProductAsync(Product updateProductDto);
        Task DeleteProductAsync(string id);
        Task<Product> CreateProductAsync(Product product);

        Task<IEnumerable<Product>> GetProductsByVendorIdAsync(string venderId);
    }
}
