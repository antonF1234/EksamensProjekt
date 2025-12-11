using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc; //giver [HttpGet, HttpPost], ok(), NotFound()
using EksamensProjekt.Models;
using EksamensProjektAPI.Repositories; // vores ProjectModel
using EksamensProjektAPI.Services; //vores Projectservice

namespace EksamensProjektAPI.Controllers;

[ApiController] // gør det til en rigtig web-API controller
[Route("api/tasksskills")] //alle endpoints starter med /api/tasksskills

public class TasksSkillsController : ControllerBase
{
    private readonly TasksSkillsRepo _repo;
    
    public TasksSkillsController(TasksSkillsRepo repo)
    {
        _repo = repo;
    }
    
    [HttpPost("create/{taskId}/{skillId}")]
    public async Task InsertTaskSkillAsync(int taskId, int skillId)
    {
        await _repo.InsertTaskSkillAsync(taskId, skillId);
    }

    [HttpDelete("delete/{taskId}/{skillId}")]
    public async Task DeleteAsync(int taskId, int skillId)
    {
        await _repo.DeleteTaskSkillAsync(taskId, skillId);
    }

    [HttpGet("get/{taskId}")]
    public async Task<IActionResult> GetAllTaskSkills(int taskId)
    {
        var skills = await _repo.GetSkillsForTaskAsync(taskId);
        return Ok(skills);
    }

}