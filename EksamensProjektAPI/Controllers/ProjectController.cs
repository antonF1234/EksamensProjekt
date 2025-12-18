using Microsoft.AspNetCore.Mvc;
using EksamensProjekt.Models; // vores ProjectModel
using EksamensProjektAPI.Services; //vores Projectservice

namespace EksamensProjektAPI.Controllers;

[ApiController] // Gør det til en rigtig web-API controller
[Route("api")] //Alle endpoints starter med /api
public class ProjectController : ControllerBase
{
    private readonly ProjectService _projectService; //Depency Injection - får ProjectService "insprøjtet" auto
    
    public ProjectController(ProjectService projectservice) // Constructor . bliver kaldt automatisk når controlleren laves
    { 
        _projectService = projectservice; //Gemmer servicen, så den kan bruges i metoden
    }

    [HttpPost ("projects")] //Opret nyt projekt , Blazor kalder denne når bruger trykker "Opret projekt"
    public async Task<IActionResult> Create(ProjectModel model)
    {
        int uid = 1; // Vi var nød til at kalde uid(user id) et eller andet
        var project = new ProjectModel
        {
            Name = model.Name,
            Description = model.Description,
            Deadline = model.Deadline,
        };
        
        await _projectService.CreateAsync(project, uid); // Sender projektet videre til service-laget. Retunere liste af projekter.
        return Ok(project);
    }

    [HttpGet("all")] // GET - hent alle projekter , fx til at vise projektlisten på forisden
    public async Task<IActionResult> GetAll()
    {
        var projects = await _projectService.GetAllAsync(); // Henter alle projekter via service-laget
        return Ok(projects);                           // Sender listen tilbage
    }

    [HttpGet("{id}")] // hent et specifikt projekt baseret på id
    public async Task<IActionResult> GetbyId(int id)
    {
        var project = await _projectService.GetByIdAsync(id); // Forsøger at hente projektet fra databasen
        
        return project == null ? NotFound() : Ok(project); // hvis projekt ikke findes - 404 not found
                                                            // hvis det findes - ok + projektet
                                                            // 201 betyder " jeg har lige lavet noget nyt og her er det fx nyt projekt"
    }

    [HttpPut("update/{id}")] // Opdaterer et eksisterende projekt. id =ID på projektet der skal opdateres. model = Opdaterede projektdata
    public async Task<IActionResult> Update(int id, ProjectModel model)
    {
        var project = await _projectService.GetByIdAsync(id); // Tjekker om projektet findes
        if (project == null) return NotFound();
        
        // Opdaterer felter
        project.Name = model.Name;
        project.Description = model.Description;
        project.Deadline = model.Deadline;
        project.Status = model.Status;

        await _projectService.UpdateAsync(project); // Sender opdateringen videre til service-laget
        return Ok();
    }

    [HttpDelete("delete/{id}")] // Sletter et projekt baseret på ID. id = ID på projektet der skal slettes
    public async Task<IActionResult> Delete(int id)

    {
        var project = await _projectService.GetByIdAsync(id); // Tjekker om projektet findes
        if (project == null) return NotFound();

        await _projectService.DeleteAsync(project);  // Sletter projektet via service-laget
        return Ok();
    }
}