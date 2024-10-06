using Microsoft.AspNetCore.Mvc;
using backend.Services;
using backend.DTOs;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        // Get all products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
        {
            var products = await _productService.GetAllProductsAsync();
            var productDtos = products.Select(p => new ProductDto
            {
                Id = p.Id,
                ProductName = p.ProductName,
                Description = p.Description,
                ProductPrice = p.ProductPrice,
                Stock = p.Stock,
                ImageUrl = p.ImageUrl,
                VendorId = p.VendorId,
                VendorName = p.VendorName
            });

            return Ok(productDtos);
        }

        // Get product by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetById(string id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            var productDto = new ProductDto
            {
                Id = product.Id,
                ProductName = product.ProductName,
                Description = product.Description,
                ProductPrice = product.ProductPrice,
                Stock = product.Stock,
                ImageUrl = product.ImageUrl,
                VendorId = product.VendorId,
                VendorName = product.VendorName
            };

            return Ok(productDto);
        }

        // Create a new product
        [HttpPost]
        public async Task<ActionResult<ProductDto>> Create([FromForm] CreateProductDto createProductDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _productService.CreateProductAsync(createProductDto);

            if (result != null)
            {
                var createdProductDto = new ProductDto
                {
                    Id = result.Id,
                    ProductName = result.ProductName,
                    Description = result.Description,
                    ProductPrice = result.ProductPrice,
                    Stock = result.Stock,
                    ImageUrl = result.ImageUrl,
                    VendorId = result.VendorId,
                    VendorName = result.VendorName
                };

                return CreatedAtAction(nameof(GetById), new { id = result.Id }, createdProductDto);
            }

            return BadRequest("Product creation failed.");
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDto>> Update(string id, [FromForm] UpdateProductDto updateProductDto)
        {


            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updatedProduct = await _productService.UpdateProductAsync(id, updateProductDto);
            if (updatedProduct == null) return NotFound();

            return Ok(updatedProduct);
        }


        // Delete a product by ID
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            await _productService.DeleteProductAsync(id);
            return NoContent();
        }

        // Get products by vendorId
        [HttpGet("vendor/{vendorId}")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetByVendorId(string vendorId)
        {
            var products = await _productService.GetProductsByVendorIdAsync(vendorId);
            if (products == null || !products.Any())
            {
                return NotFound($"No products found for vendor ID {vendorId}");
            }

            return Ok(products);
        }
    }
}
