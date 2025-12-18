namespace EksamensProjektAPI.Controllers;

using Microsoft.AspNetCore.Mvc; //giver [HttpGet, HttpPost], ok(), NotFound()
using EksamensProjektAPI.Services; //vores Projectservice


[ApiController]
[Route("api/usersskills")] // base url for all endpoints

public class UsersSkillsController : ControllerBase // arv alt fra den standard controllerbase, giver mulighed for at bruge fx [HttpGet, HttpPost]
{
    private readonly UsersSkillsService _usersSkillsService; // laver en readonly reference til UsersSkillsService
    
    public UsersSkillsController(UsersSkillsService usersSkillsService) // Constructoren kaldes automatisk, når controlleren oprettes af DI-containeren
    {
        _usersSkillsService = usersSkillsService;
        // Gemmer den injicerede service i et privat felt, så den kan bruges i controllerens metoder
    }


    [HttpPost("adduserskill/{skillId}/{userId}")]
    public async Task<IActionResult> CreateAsyncUserSkill(int skillId, int userId) // metoden tager skillId og userId som parameter
    {
        await _usersSkillsService.InsertUserSkillAsync(skillId, userId); // kalder InsertUserSkillAsync fra servicen med skillId og userId
        return Ok();
    }
    
    [HttpGet("getuserskills/{userId}")]
    public async Task<IActionResult> GetAllAsyncUserSkills(int userId) // metoden tager en userId og returnerer en liste med alle skills
    {
        var skills = await _usersSkillsService.GetAllUserSkillsAsync(userId); // kalder GetAllUserSkillsAsync fra servicen med userId
        return Ok(skills); // returnerer listen af objekter tilbage
    }
    
    [HttpDelete("deleteuserskill/{skillId}/{userId}")]
    public async Task<IActionResult> DeleteAsyncUserSkill(int skillId, int userId) // metoden tager skillId og userId som parameter for at slette en skill på en bruger
    {
        await _usersSkillsService.DeleteUserSkillAsync(skillId, userId); // kalder DeleteUserSkillAsync fra servicen med skillId og userId
        return Ok();
    }
}