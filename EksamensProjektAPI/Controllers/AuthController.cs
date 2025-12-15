using Microsoft.AspNetCore.Mvc;
using EksamensProjekt.Models;
using EksamensProjektAPI.Services;

namespace EksamensProjektAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserModel user)
    {
        var loggedInUser = await _authService.LoginAsync(user.Username, user.Password);
        if (loggedInUser is null) 
            return Unauthorized();

        return Ok(new 
        { 
            loggedInUser.UserId, 
            loggedInUser.Username, 
            loggedInUser.IsAdmin 
        });
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register(UserModel user)
    {
        await _authService.RegisterAsync(user.Username, user.Password, user.Email, user.IsAdmin);
        return Ok();
    }
}