using backend.Interfaces;
using backend.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using backend.Models;
using backend.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http.HttpResults;

namespace backend.Controllers
{
    
    [Route("api/[controller]")] // Specifies the route template for controller
    [ApiController] // Automatic model validation enabled
    [Authorize(Roles = "Administrator")]

    public class VendorController(VendorService vendorService) : ControllerBase
    {
        private readonly VendorService _vendorService = vendorService;

    // Get all Vendors

    [HttpGet]
    public async Task<ActionResult<IEnumerable<VendorDTO>>> GetAllVendors()
    {
        var vendors = await _vendorService.GetAllVendorsDTOAsync();

        if(vendors == null || vendors.Count == 0)
            return BadRequest("No Vendors found!");
        
        return Ok(vendors); // Return 200 ok with list of VendorDTO
    }

    // Get paticular Vendor

    [HttpGet("{id}")]
    public async Task<ActionResult<VendorDTO>> GetVendorById(string id)
    {
        var vendor = await _vendorService.GetVendorByIdDTOAsync(id);

        if(vendor == null) 
            return BadRequest("Paticular Vendor not found!");

        return Ok(vendor);
    }

    // Create new Vendor

    [HttpPost]
    public async Task<ActionResult<VendorDTO>> CreateVendor([FromBody] CreateVendorDTO createVendorDTO)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);

        var newVendor = await _vendorService.CreateVendorDTOAsync(createVendorDTO);

        if(newVendor != null)
        {
            var createdVendorDTO = new VendorDTO
            {
                Id = newVendor.Id,
                VendorName = newVendor.VendorName,
                VendorEmail = newVendor.VendorEmail,
                VendorPhone = newVendor.VendorPhone,
                VendorAddress = newVendor.VendorAddress,
                VendorCity = newVendor.VendorCity
            };

            return CreatedAtAction(nameof(GetVendorById), new{ id = newVendor.Id }, createdVendorDTO);
        }

        return BadRequest("Vendor creation failed!");
    }

    // Update existing Vendor

    [HttpPut("{id}")]

    public async Task<ActionResult<VendorDTO>> UpdateVendor(string id, [FromBody] UpdateVendorDTO updateVendorDTO)
    {

        if(id != updateVendorDTO.Id) return BadRequest("Id mismatch!");

        // Check model binding and validation succeed
        if(!ModelState.IsValid) return BadRequest(ModelState);

        var updatedVendor = await _vendorService.UpdateVendorDTOAsync(updateVendorDTO);

        if(updatedVendor == null) return NotFound();

        var updatedVendorDTO = new VendorDTO
        {
            Id = updatedVendor.Id,
            VendorName = updatedVendor.VendorName,
            VendorEmail = updatedVendor.VendorEmail,
            VendorPhone = updatedVendor.VendorPhone,
            VendorAddress = updatedVendor.VendorAddress,
            VendorCity = updatedVendor.VendorCity
        };

        return Ok();  
    }

    // Delete existing Vendor

    [HttpDelete("{id}")]

    public async Task<ActionResult> DeleteVendor(string id)
    {
        var result = await _vendorService.GetVendorByIdDTOAsync(id);

        if(result == null) NotFound();

        await _vendorService.DeleteVendorDTOAsync(id);
        return NoContent(); // 204 successfully deleted, no response body
    }

    }

}