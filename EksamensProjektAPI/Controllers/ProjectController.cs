using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc; //giver [HttpGet, HttpPost], ok(), NotFound()
using EksamensProjekt.Models; // vores ProjectModel
using EksamensProjektAPI.Services; //vores Projectservice

namespace EksamensProjektAPI.Controllers;

[ApiController] // gør det til en rigtig web-API controller
[Route("api")] //alle endpoints starter med /api
public class ProjectController : ControllerBase
{
    private readonly ProjectService _projectService; //Depency Injection - får ProjectService "insprøjtet" auto
    
    public ProjectController(ProjectService projectservice) // Constructor . bliver kaldt automatisk når controlleren laves
    { 
        _projectService = projectservice; //gemmer servicen, så den kan bruges i metoden
    }

    [HttpPost ("projects")] //Opret nyt projekt , Blazor kalder denne når bruger trykker "Opret projekt"
    public async Task<IActionResult> Create(ProjectModel model)
    {
        int uid = 1; // vi var nød til at kalde det et eller andet
        var project = new ProjectModel
        {
            Name = model.Name,
            Description = model.Description,
            Deadline = model.Deadline,
        };
        
        await _projectService.CreateAsync(project, uid); // sender projektet til servicen;
        return Ok(project);
    }

    [HttpGet("all")] // GET - hent alle projekter , fx til at vise projektlisten på forisden
    public async Task<IActionResult> GetAll()
    {
        var projects = await _projectService.GetAllAsync(); //henter alle fra databasen
        return Ok(projects);                           // statuskode 200 + sender listen tilbage
    }

    [HttpGet("{id}")] // hent et specifikt projekt med id = 5
    public async Task<IActionResult> GetbyId(int id)
    {
        var project = await _projectService.GetByIdAsync(id); // søger i databasen
        
        return project == null ? NotFound() : Ok(project); // hvis projekt ikke findes - 404 not found
                                                            // hvis det findes - 200 ok + projektet
                                                            // 201 betyder " jeg har lige lavet noget nyt og her er det fx nyt projekt"
    }
}