using backend.Interfaces;
using backend.DTOs;
using Microsoft.AspNetCore.Mvc;
using backend.Models;
using backend.Services;


// [Authorize(Roles = "Administrator")]
[Route("api/[controller]")] // Specifies the route template for controller
[ApiController] // Automatic model validation enabled
public class VendorController : ControllerBase
{
    private readonly VendorService _vendorService;
    private readonly EmailService _emailService;


    public VendorController(VendorService vendorService, EmailService emailService)
    {
        _vendorService = vendorService;
        _emailService = emailService;
    }

    // Get all Vendors
    [HttpGet]
    public async Task<ActionResult<IEnumerable<VendorDTO>>> GetAllVendors()
    {
        var vendors = await _vendorService.GetAllVendorsDTOAsync();
        var vendorDtos = vendors.Select(v => new VendorDTO
        {
            Id = v.Id,
            VendorName = v.VendorName,
            VendorEmail = v.VendorEmail,
            VendorPhone = v.VendorPhone,
            VendorAddress = v.VendorAddress,
            VendorCity = v.VendorCity,
            IsActive = v.IsActive,
            Products = v.Products,
            Feedbacks = v.Feedbacks
        });

        return Ok(vendorDtos);
    }

    // Get paticular Vendor

    [HttpGet("{id}")]
    [ActionName("GetVendorById")]
    public async Task<ActionResult<VendorDTO>> GetVendorById(string id)
    {
        var vendor = await _vendorService.GetVendorByIdDTOAsync(id);

        if (vendor == null) return NotFound();

        var singleVendorDTO = new VendorDTO
        {
            Id = vendor.Id,
            VendorName = vendor.VendorName,
            VendorEmail = vendor.VendorEmail,
            VendorPhone = vendor.VendorPhone,
            VendorAddress = vendor.VendorAddress,
            VendorCity = vendor.VendorCity,
            IsActive = vendor.IsActive,
            Products = vendor.Products,
            Feedbacks = vendor.Feedbacks
        };

        return Ok(singleVendorDTO);
    }

    // Login for vendor

    [HttpPost("login")]

    public async Task<ActionResult> Login([FromBody] VendorLoginDTO vendorLoginDTO)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var vendorDto = await _vendorService.LoginAsync(vendorLoginDTO);
            return Ok(vendorDto);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    // Create new Vendor
    [HttpPost("register")]
    public async Task<ActionResult<VendorDTO>> CreateVendor([FromBody] CreateVendorDTO createVendorDTO)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var newVendor = await _vendorService.CreateVendorDTOAsync(createVendorDTO);

        if (newVendor != null)
        {
            return CreatedAtAction(nameof(GetVendorById), new { id = newVendor.Id }, newVendor);
        }

        return BadRequest("Vendor creation failed!");
    }

    // Update existing Vendor

    [HttpPut("{id}")]
    public async Task<ActionResult<VendorDTO>> UpdateVendor(string id, [FromBody] UpdateVendorDTO updateVendorDTO)
    {

        // Check model binding and validation succeed
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var updatedVendor = await _vendorService.UpdateVendorAsync(id, updateVendorDTO);

        if (updatedVendor == null) return NotFound();

        return Ok(updatedVendor);
    }


    // Delete existing Vendor
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteVendor(string id)
    {
        var result = await _vendorService.GetVendorByIdDTOAsync(id);

        if (result == null) NotFound();

        await _vendorService.DeleteVendorDTOAsync(id);
        //return NoContent(); // 204 successfully deleted, no response body

        return Ok(new { message = "Vendor successfully deleted!" });
    }


    [HttpPost("{vendorId}/feedback")]
    public async Task<IActionResult> AddFeedback(string vendorId, [FromBody] CustomerFeedback feedback)
    {
        try
        {
            await _vendorService.AddFeedbackToVendorAsync(vendorId, feedback);
            return Ok("Feedback added successfully.");
        }
        catch (KeyNotFoundException)
        {
            return NotFound("Vendor not found.");
        }
        catch (ApplicationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error: " + ex.Message);
        }
    }
}