namespace EksamensProjektAPI.Controllers;

using Microsoft.AspNetCore.Mvc; //giver [HttpGet, HttpPost], ok(), NotFound()
using EksamensProjektAPI.Services; //vores Projectservice


[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    
    public UserController(UserService userService) // Service der indeholder forretningslogik for brugere
    {
        _userService = userService; // Constructor med Dependency Injection af UserService.
    }
    
    //Henter alle brugere fra systemet. Retunerer liste af brugere
    [HttpGet("allusers")]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllAsync(); // Henter alle brugere via service-laget
        if (users == null) return NotFound(); // Hvis ingen brugere findes returneres 404
        return Ok(users); // Returnerer listen af brugere
    }
}