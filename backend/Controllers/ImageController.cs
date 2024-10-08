// IT21470004 - BOPITIYA S. R. - IMAGE CONTROLLER

using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly CloudinaryService _cloudinaryService;

        public ImageController(CloudinaryService cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;
        }

        // Upload image
        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file)
        {
            var result = await _cloudinaryService.UploadImageAsync(file);

            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }

            return Ok(new { Url = result.SecureUrl });
        }
    }
}
