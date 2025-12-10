namespace EksamensProjektAPI.Controllers;

using Microsoft.AspNetCore.Mvc; //giver [HttpGet, HttpPost], ok(), NotFound()
using EksamensProjekt.Models; // vores ProjectModel
using EksamensProjektAPI.Services; //vores Projectservice


[ApiController]
[Route("api/userstasks")]

public class UsersTasksController : ControllerBase
{
    private readonly UsersTasksService _userTasksService;
    
    public UsersTasksController(UsersTasksService usersTasksService)
    {
        _userTasksService = usersTasksService;
    }
    
    [HttpPost("addusertask/{taskId}/{userId}")]
    public async Task<IActionResult> AddUserToTask(int taskId, int userId)
    {
        await _userTasksService.InsertUserTaskAsync(taskId, userId);
        return Ok();
    }
    
    [HttpGet("getuserstasks/{userId}")]
    public async Task<IActionResult> GetAllUserTasks(int userId)
    {
        var tasks = await _userTasksService.GetAllUserTasksAsync(userId);
        return Ok(tasks);
    }
    
}