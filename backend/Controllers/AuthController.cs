// IT21470004 - BOPITIYA S. R. - Authentication Controller

using backend.DTOs;
using backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterDTO userRegisterDTO)
    {
        var result = await _authService.Register(userRegisterDTO);


        if (result.Succeeded)
        {
            return Ok(new { message = "User registered successfully!" });
        }

        return BadRequest(result.Errors);
    }

    // Login endpoint
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(UserLoginDTO userLoginDTO)
    {
        var userLoginResponse = await _authService.Login(userLoginDTO);
        if (userLoginResponse != null)
        {
            return Ok(new UserLoginResponseDTO
            {
                UserId = userLoginResponse.UserId,  // Convert ObjectId or Guid to string
                Firstname = userLoginResponse.Firstname,
                Lastname = userLoginResponse.Lastname,
                Role = userLoginResponse.Role,
                Token = userLoginResponse.Token
            });
        }

        return Unauthorized("Invalid login attempt.");
    }

    // Logout endpoint
    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _authService.Logout();
        return Ok(new { message = "User logged out successfully!" });
    }

    [HttpPut("update/{userId}")]
    [Authorize]
    public async Task<IActionResult> UpdateUser(Guid userId, UserUpdateDTO userUpdateDTO)
    {

        var result = await _authService.UpdateUser(userId, userUpdateDTO);

        if (result.Succeeded)
        {
            return Ok(new { message = "User updated successfully!" });
        }

        return BadRequest(result.Errors);

    }


}
