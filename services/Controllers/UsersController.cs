using Microsoft.AspNetCore.Mvc;
using web_service.Models;
using web_service.Services;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;


namespace web_service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly string _hmacSecretKey;

        public UsersController(UserService userService, IConfiguration configuration)
        {
            _userService = userService;
            // Fetch the HMAC secret key from the .env or appsettings file
            _hmacSecretKey = Environment.GetEnvironmentVariable("HMAC_SECRET_KEY");
        }

        // GET: api/Users
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var users = await _userService.GetAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving users.", error = ex.Message });
            }
        }

        // GET: api/Users/{id}
        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> Get(string id)
        {   
            try
            {
                var user = await _userService.GetAsync(id);

                if (user == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the user.", error = ex.Message });
            }
        }

        // POST: api/Users
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User newUser)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage).ToList();

                return BadRequest(new { message = "Validation failed.", errors });
            }

            // Check if password is provided
            if (string.IsNullOrEmpty(newUser.Password))
            {
                return BadRequest(new { message = "Password is required." });
            }

            try
            {
                // Hash the password before saving
                newUser.Password = HashPassword(newUser.Password);

                await _userService.CreateAsync(newUser);
                return Ok(new { message = "User created successfully.", user = newUser });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the user.", error = ex.Message });
            }
        }

        // PUT: api/Users/{id}
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, [FromBody] User updatedUser)
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
                var user = await _userService.GetAsync(id);

                if (user == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                updatedUser.UserId = user.UserId; // Keep the original UserId

                // Hash the password if it's being updated
                if (!string.IsNullOrEmpty(updatedUser.Password))
                {
                    updatedUser.Password = HashPassword(updatedUser.Password);
                }

                await _userService.UpdateAsync(id, updatedUser);
                return Ok(new { message = "User updated successfully.", user = updatedUser });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the user.", error = ex.Message });
            }
        }

        // PATCH: api/Users/{id}
        [HttpPatch("{id:length(24)}")]
        public async Task<IActionResult> PatchUpdate(string id, [FromBody] JsonPatchDocument<User> patchDocument)
        {
            if (patchDocument == null)
            {
                return BadRequest(new { message = "Invalid patch document." });
            }

            try
            {
                var user = await _userService.GetAsync(id);

                if (user == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                // Apply the patch to the user entity
                patchDocument.ApplyTo(user, ModelState);

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage).ToList();

                    return BadRequest(new { message = "Validation failed.", errors });
                }

                // Check if the Password field is part of the patch operations
                var passwordOperation = patchDocument.Operations.FirstOrDefault(op => op.path == "/Password");

                if (passwordOperation != null && !string.IsNullOrEmpty(user.Password))
                {
                    // Hash the password only if it's being updated
                    user.Password = HashPassword(user.Password);
                }
                else
                {
                    // Keep the original password if it's not being updated
                    var existingUser = await _userService.GetAsync(id);
                    user.Password = existingUser.Password;
                }

                await _userService.UpdateAsync(id, user);

                return Ok(new { message = "User updated successfully.", user });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the user.", error = ex.Message });
            }
        }


        // DELETE: api/Users/{id}
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var user = await _userService.GetAsync(id);

                if (user == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                await _userService.RemoveAsync(id);
                return Ok(new { message = "User deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the user.", error = ex.Message });
            }
        }

        // POST: api/Users/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest(new { message = "Username and password are required." });
            }

            try
            {
                var user = await _userService.GetByUsernameAsync(loginRequest.Username);

                if (user == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                // Verify password
                if (!VerifyPassword(loginRequest.Password, user.Password))
                {
                    return BadRequest(new { message = "Invalid username or password." });
                }


                // Check user status, only allow login if status is "Accept"
                if (user.Status != Status.Accept)
                {
                    return BadRequest(new { message = $"Login not allowed. Your account status is '{user.Status}'." });
                }
                

                // Return success response
                return Ok(new
                {
                    message = "Login successful",
                    username = user.Username,
                    role = user.Role,
                    userId = user.UserId
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while logging in.", error = ex.Message });
            }
        }

        // Utility function to hash passwords using HMACSHA256
        private string HashPassword(string password)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_hmacSecretKey)))
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashedBytes = hmac.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hashedBytes);
            }
        }

        // Verify password by hashing the input password and comparing it with the stored hash
        private bool VerifyPassword(string inputPassword, string storedPassword)
        {
            string hashedInputPassword = HashPassword(inputPassword);
            return hashedInputPassword == storedPassword;
        }
    }

    // Login request model
    public class LoginRequest
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
