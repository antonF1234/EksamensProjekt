using Microsoft.AspNetCore.Mvc;
using EksamensProjekt.Models; // vores ProjectModel
using EksamensProjektAPI.Services; //vores Projectservice

namespace EksamensProjektAPI.Controllers;

[ApiController]
[Route("api/skills")] // base url for all endpoints

public class SkillController  : ControllerBase // arv alt fra den standard controllerbase, giver mulighed for at bruge fx [HttpGet, HttpPost]
{
    private readonly SkillService _skillService; // laver en readonly reference til skillService

    public SkillController(SkillService skillService) // Constructoren kaldes automatisk, når controlleren oprettes af DI-containeren
    {
        _skillService = skillService;
        // Gemmer den injicerede service i et privat felt, så den kan bruges i controllerens metoder
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(SkillModel model) // Metoden tager en SkillModel som parameter
    {
        var skill = new SkillModel // instansiere et nyt SkillModel
        {
            Name = model.Name, // sæt Name til model.Name
        };
        
        await _skillService.CreateAsync(skill); // kalder skillService.CreateAsync med det nye SkillModel
        return Ok(model); // returnerer den sendte model tilbage til brugeren
    }
    
    [HttpGet("allskills")]
    public async Task<IActionResult> GetAllAsync() // Metoden returnerer en liste med alle skills
    {
        var skills = await _skillService.GetAllAsync(); // kalder skillService.GetAllAsync
        return Ok(skills); // returnerer listen tilbage til brugeren
    }
}