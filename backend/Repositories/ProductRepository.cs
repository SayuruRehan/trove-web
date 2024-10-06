using backend.Interfaces;
using backend.Models;
using MongoDB.Driver;


namespace backend.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _product;

        public ProductRepository(IMongoDatabase database)
        {
            _product = database.GetCollection<Product>("Products");
        }

        // Retrieve all products
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            try
            {
                return await _product.Find(FilterDefinition<Product>.Empty).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error retrieving products", ex);
            }
        }

        // Retrieve a product by its ID
        public async Task<Product> GetProductByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Invalid product ID.");

            try
            {
                var filter = Builders<Product>.Filter.Eq(x => x.Id, id);
                return await _product.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error retrieving product with ID {id}", ex);
            }
        }

        // Create a new product
        public async Task<Product> CreateProductAsync(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            try
            {
                await _product.InsertOneAsync(product); // MongoDB generates the ID
                return product; // Return the product with the generated ID
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error creating the product.", ex);
            }
        }

        // Update an existing product
        public async Task<Product> UpdateProductAsync(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            if (string.IsNullOrEmpty(product.Id))
                throw new ArgumentException("Invalid product ID.");

            var filter = Builders<Product>.Filter.Eq(p => p.Id, product.Id);

            try
            {
                // Use FindOneAndReplaceOptions to return the updated document
                var options = new FindOneAndReplaceOptions<Product>
                {
                    ReturnDocument = ReturnDocument.After // Ensures the updated document is returned
                };

                var result = await _product.FindOneAndReplaceAsync(filter, product, options)
                             ?? throw new KeyNotFoundException($"Product with ID {product.Id} not found.");

                return result; // Return the updated product
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error updating product with ID {product.Id}", ex);
            }
        }


        // Delete a product by its ID
        public async Task DeleteProductAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Invalid product ID.");

            var filter = Builders<Product>.Filter.Eq(p => p.Id, id);

            try
            {
                var result = await _product.DeleteOneAsync(filter);

                if (result.DeletedCount == 0)
                    throw new KeyNotFoundException($"Product with ID {id} not found.");
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error deleting product with ID {id}", ex);
            }
        }

        // Retrieve products by vendorId
        public async Task<IEnumerable<Product>> GetProductsByVendorIdAsync(string vendorId)
        {
            if (string.IsNullOrEmpty(vendorId))
                throw new ArgumentException("Invalid vendor ID.");

            var filter = Builders<Product>.Filter.Eq(p => p.VendorId, vendorId);
            try
            {
                return await _product.Find(filter).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error retrieving products for vendor ID {vendorId}", ex);
            }
        }

    }
}
