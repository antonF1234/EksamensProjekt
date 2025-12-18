namespace EksamensProjektAPI.Controllers;

using Microsoft.AspNetCore.Mvc;         // Til [HttpGet] osv
using EksamensProjektAPI.Services;     // Vores UsersTasksService

[ApiController]                         
[Route("api/userstasks")]                
public class UsersTasksController : ControllerBase
{
    // Vi får en UsersTasksService automatisk via dependency injection
    private readonly UsersTasksService _userTasksService;

    // Constructor,  ASP.NET giver os servicen med det samme
    public UsersTasksController(UsersTasksService usersTasksService)
    {
        _userTasksService = usersTasksService;  // Gemmer den så vi kan bruge den i metoderne
    }

    // POST: /api/userstasks/addusertask/{taskId}/{userId}
    // Bruges fra Blazor når admin trykker "Tilføj bruger til opgave"
    [HttpPost("addusertask/{taskId}/{userId}")]
    public async Task<IActionResult> AddUserToTask(int taskId, int userId)
    {
        // Tilføjer brugeren til opgaven via servicen
        await _userTasksService.InsertUserTaskAsync(taskId, userId);

        // Sender "OK" tilbage, frontend opdaterer selv listen
        return Ok();
    }

    // GET: /api/userstasks/getuserstasks/{userId}
    // Henter alle opgaver for en bestemt bruger
    [HttpGet("getuserstasks/{userId}")]
    public async Task<IActionResult> GetAllUserTasks(int userId)
    {
        // Henter listen via servicen
        var tasks = await _userTasksService.GetAllUserTasksAsync(userId);

        // Sender listen tilbage som JSON
        return Ok(tasks);
    }

    // GET: /api/userstasks/getusersfromtask/{taskId}
    // Bruges på Tasks-siden til at vise hvilke brugere der er på en opgave 
    [HttpGet("getusersfromtask/{taskId}")]
    public async Task<IActionResult> GetAllUsersFromTask(int taskId)
    {
        // Henter alle brugere på den opgave
        var users = await _userTasksService.GetAllUsersOnTask(taskId);

        // Sender listen med brugere tilbage – Blazor viser dem som badges med kryds
        return Ok(users);
    }

    // DELETE: /api/userstasks/deluserfromtask/{taskId}/{userId}
    // Bruges når man trykker på krydset, for at fjerne en bruger fra en opgave
    [HttpDelete("deluserfromtask/{taskId}/{userId}")]
    public async Task<IActionResult> DeleteAsyncUserSkill(int taskId, int userId)
    {
        // Sletter rækken i users_tasks-tabellen via servicen (når man trykker kryds
        await _userTasksService.DeleteUserTaskAsync(taskId, userId);

        // Sender "OK" tilbage, frontend opdaterer listen selv
        return Ok();
    }
}