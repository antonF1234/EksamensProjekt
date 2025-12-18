using Microsoft.AspNetCore.Mvc; // Til alle de der IActionResult og HttpPost osv.
using EksamensProjekt.Models;

namespace EksamensProjektAPI.Controllers;

[ApiController] // gør det til en rigtig web API controller
[Route("api/auth")] // endpoints starter med denne rute
public class AuthController : ControllerBase
{
    // authservice gør alt det svære, kodeord, snakke med databasen osv.
    private readonly AuthService _authService;

    //constructoren – den kører helt automatisk når nogen beder om denne controller
    // ASP.NET giver os en authservice med det samme, (dependency injection)
    public AuthController(AuthService authService)
    {
        _authService = authService; //gemmer servicen så vi kan bruge den i metoden
    }

    // POST: /api/auth/login
    // bliver kaldt fra login siden i blazor når login trykkes   
    [HttpPost("login")]
    public async Task<IActionResult> Login(UserModel user)
    {
        // sender kode og brugernavn videre til AUTHservice
        // Den tjekker i databasen om brugeren findes og om kodeordet passer (med hashing selvfølgelig)
        var loggedInUser = await _authService.LoginAsync(user.Username, user.Password); // Tjekker brugernavn og kodeord i databasen via AuthService
        //hvis ingen bruger returneres, forkert brugernavn eller kode..
        if (loggedInUser is null) 
            //sender "Unauthorized" tilbage
            return Unauthorized();
        // hvis det gik godt sender vi de vigtige brugerdata tilbage
        return Ok(new UserModel()
        { 
            UserId = loggedInUser.UserId,
            Username = loggedInUser.Username,
            Email = loggedInUser.Email,
            IsAdmin = loggedInUser.IsAdmin,            
        });
    }


    // POST: /api/auth/register
    // her kan admin oprette nye brugere
    [HttpPost("register")]
    public async Task<IActionResult> Register(UserModel user)
    {
        await _authService.RegisterAsync(user.Username, user.Password, user.Email, user.IsAdmin); // Opretter en ny bruger i databasen via AuthService
        return Ok();
    }
}