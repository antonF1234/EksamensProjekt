using Microsoft.AspNetCore.Mvc;
using EksamensProjektAPI.Services;

namespace EksamensProjektAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(UserService userService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        await userService.RegisterAsync(dto.Username, dto.Password, dto.Email, dto.IsAdmin);
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await userService.LoginAsync(dto.Username, dto.Password);
        if (user == null) 
            return Unauthorized();

        return Ok();
    }
}

// DTOs (data transfer objects)
public record RegisterDto(string Username, string Password, string Email, bool IsAdmin);
public record LoginDto(string Username, string Password);