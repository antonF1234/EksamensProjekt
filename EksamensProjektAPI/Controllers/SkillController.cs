using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc; //giver [HttpGet, HttpPost], ok(), NotFound()
using EksamensProjekt.Models; // vores ProjectModel
using EksamensProjektAPI.Services; //vores Projectservice

namespace EksamensProjektAPI.Controllers;

[ApiController]
[Route("api/skills")]

public class SkillController  : ControllerBase
{
    private readonly SkillService _skillService;

    public SkillController(SkillService skillService)
    {
        _skillService = skillService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(SkillModel model)
    {
        var skill = new SkillModel
        {
            Name = model.Name,
        };
        
        await _skillService.CreateAsync(skill);
        return Ok(model);
    }
    
    [HttpGet("allskills")]
    public async Task<IActionResult> GetAllAsync()
    {
        var skills = await _skillService.GetAllAsync();
        return Ok(skills);
    }
}