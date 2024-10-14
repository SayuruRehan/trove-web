using Microsoft.AspNetCore.Mvc;
using web_service.Models;
using web_service.Services;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace web_service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;
        // private readonly VendorService _vendorService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
            // _vendorService = vendorService;
        }

        // ------------------ GET: api/products ------------------
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var products = await _productService.GetAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(500, new { message = "An error occurred while retrieving products.", error = ex.Message });
            }
        }

        // ------------------ GET: api/products/{id} ------------------
        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var product = await _productService.GetAsync(id);

                if (product == null)
                {
                    return NotFound(new { message = "Product not found." });
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(500, new { message = "An error occurred while retrieving the product.", error = ex.Message });
            }
        }

        // ------------------ GET: api/products/by-category/{categoryId} ------------------
        [HttpGet("by-category/{categoryId:length(24)}")]
        public async Task<IActionResult> GetProductsByCategory(string categoryId)
        {
            try
            {
                var products = await _productService.GetByCategoryIdAsync(categoryId);

                if (products == null || products.Count == 0)
                {
                    return NotFound(new { message = "No products found for the specified category." });
                }

                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving products by category.", error = ex.Message });
            }
        }

        // ------------------ POST: api/products ------------------
        [HttpPost]
public async Task<IActionResult> Create([FromBody] Product newProduct)
{
    if (!ModelState.IsValid)
    {
        var errors = ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage).ToList();

        return BadRequest(new { message = "Validation failed.", errors });
    }

    try
    {
        // Save product to database (including Base64 image string)
        await _productService.CreateAsync(newProduct);
        return Ok(new { message = "Product created successfully.", product = newProduct });
    }
    catch (Exception ex)
    {
        // Log the exception as needed
        return StatusCode(500, new { message = "An error occurred while creating the product.", error = ex.Message });
    }
}


        // ------------------ PUT: api/products/{id} ------------------
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, [FromBody] Product updatedProduct)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList();

                return BadRequest(new { message = "Validation failed.", errors });
            }

            try
            {
                var product = await _productService.GetAsync(id);

                if (product == null)
                {
                    return NotFound(new { message = "Product not found." });
                }

                updatedProduct.ProductId = product.ProductId;

                await _productService.UpdateAsync(id, updatedProduct);

                return Ok(new { message = "Product updated successfully.", product = updatedProduct });
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(500, new { message = "An error occurred while updating the product.", error = ex.Message });
            }
        }

        // ------------------ DELETE: api/products/{id} ------------------
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var product = await _productService.GetAsync(id);

                if (product == null)
                {
                    return NotFound(new { message = "Product not found." });
                }

                await _productService.RemoveAsync(id);

                return Ok(new { message = "Product deleted successfully." });
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(500, new { message = "An error occurred while deleting the product.", error = ex.Message });
            }
        }
    }
}
