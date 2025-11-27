using Microsoft.AspNetCore.Mvc;
using EksamensProjektAPI.Services;

namespace EksamensProjektAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;

    public AuthController(UserService userService) => _userService = userService;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        await _userService.RegisterAsync(dto.Username, dto.Password, dto.Email, dto.IsAdmin);
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var user = await _userService.LoginAsync(dto.Username, dto.Password);
        return user is not null ? Ok(user) : Unauthorized();
    }
}

// DTOs (data transfer objects)
public record RegisterDto(string Username, string Password, string Email, bool IsAdmin);
public record LoginDto(string Username, string Password);