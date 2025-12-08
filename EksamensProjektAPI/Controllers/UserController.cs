namespace EksamensProjektAPI.Controllers;

using Microsoft.AspNetCore.Mvc; //giver [HttpGet, HttpPost], ok(), NotFound()
using EksamensProjekt.Models; // vores ProjectModel
using EksamensProjektAPI.Services; //vores Projectservice


[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    
    public UserController(UserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet("allusers")]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllAsync();
        if (users == null) return NotFound();
        return Ok(users);                           // statuskode 200 + sender listen tilbage
    }
}