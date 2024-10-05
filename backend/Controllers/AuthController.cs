using backend.DTOs;
using backend.Interfaces;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IAuthService _authService;

    public UserController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterDTO userRegisterDTO)
    {
        var result = await _authService.Register(userRegisterDTO);

        System.Console.WriteLine(userRegisterDTO);

        if (result.Succeeded)
        {
            return Ok(new { message = "User registered successfully!" });
        }

        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginDTO userLoginDTO)
    {
        var token = await _authService.Login(userLoginDTO);
        if (token != null)
        {
            return Ok(new { token });
        }

        return Unauthorized("Invalid login attempt.");
    }
}
