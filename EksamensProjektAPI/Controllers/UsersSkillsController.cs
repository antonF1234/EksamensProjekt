namespace EksamensProjektAPI.Controllers;

using Microsoft.AspNetCore.Mvc; //giver [HttpGet, HttpPost], ok(), NotFound()
using EksamensProjekt.Models; // vores ProjectModel
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
    
    [HttpDelete("deleteuserskill/{userSkillId}")]
    public async Task<IActionResult> DeleteAsyncUserSkill(int userSkillId)
    {
        await _usersSkillsService.DeleteUserSkillAsync(userSkillId);
        return Ok();
    }
}