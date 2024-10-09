// IT21470004 - BOPITIYA S. R. - USER CONTROLLER

using backend.DTOs;
using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();

                if (users == null)
                {
                    return NotFound("No users found.");
                }

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/User/vendors
        [HttpGet("vendors")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllVendors()
        {
            try
            {
                var vendors = await _userService.GetAllVendorsAsync();

                if (vendors == null)
                {
                    return NotFound("No vendors found.");
                }

                return Ok(vendors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPatch("approve/{id}")]
        public async Task<IActionResult> ApproveUser(string id)
        {
            try
            {
                var result = await _userService.ApproveUser(id);
                if (result.Succeeded)
                {
                    return Ok(new { message = "User updated successfully!" });
                }

                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        // // PATCH : api/user/deactivate
        // [HttpPatch("deactivate/{id}")]
        // public async Task<IActionResult> DeactivateUser(string id)
        // {
        //     if (string.IsNullOrEmpty(id))
        //     {
        //         return BadRequest("User ID cannot be null or empty.");
        //     }

        //     try
        //     {
        //         var result = await _userService.DeactivateUser(id);
        //         if (result.Succeeded)
        //         {
        //             return Ok(new { message = "User deactivated successfully!" });
        //         }

        //         return BadRequest(result.Errors);
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(500, $"Internal server error: {ex.Message}");
        //     }
        // }


        // GET: api/User/unApproved
        [HttpGet("unApproved")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUnApprovedUsers()
        {
            try
            {
                var users = await _userService.GetAllUnApprovedUsersAsync();

                if (users == null)
                {
                    return NotFound("No users found.");
                }

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/User/unApproved
        [HttpGet("vendors/unApproved")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUnApprovedVendors()
        {
            try
            {
                var users = await _userService.GetAllUnApprovedVendorsAsync();

                if (users == null)
                {
                    return NotFound("No vendors found.");
                }

                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
