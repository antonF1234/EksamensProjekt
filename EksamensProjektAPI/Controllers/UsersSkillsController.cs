namespace EksamensProjektAPI.Controllers;

using Microsoft.AspNetCore.Mvc; //giver [HttpGet, HttpPost], ok(), NotFound()
using EksamensProjektAPI.Services; //vores Projectservice


[ApiController]
[Route("api/usersskills")]

public class UsersSkillsController : ControllerBase
{
    private readonly UsersSkillsService _usersSkillsService;
    
    public UsersSkillsController(UsersSkillsService usersSkillsService)
    {
        _usersSkillsService = usersSkillsService;
    }


    [HttpPost("adduserskill/{skillId}/{userId}")]
    public async Task<IActionResult> CreateAsyncUserSkill(int skillId, int userId)
    {
        await _usersSkillsService.InsertUserSkillAsync(skillId, userId);
        return Ok();
    }
    
    [HttpGet("getuserskills/{userId}")]
    public async Task<IActionResult> GetAllAsyncUserSkills(int userId)
    {
        var skills = await _usersSkillsService.GetAllUserSkillsAsync(userId);
        return Ok(skills);
    }
    
    [HttpDelete("deleteuserskill/{skillId}/{userId}")]
    public async Task<IActionResult> DeleteAsyncUserSkill(int skillId, int userId)
    {
        await _usersSkillsService.DeleteUserSkillAsync(skillId, userId);
        return Ok();
    }
}