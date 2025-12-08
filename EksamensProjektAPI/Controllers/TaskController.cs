using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc; //giver [HttpGet, HttpPost], ok(), NotFound()
using EksamensProjekt.Models; // vores ProjectModel
using EksamensProjektAPI.Services; //vores Projectservice

namespace EksamensProjektAPI.Controllers;

[ApiController] // gør det til en rigtig web-API controller
[Route("api/tasks")] //alle endpoints starter med /api/tasks
public class TaskController : ControllerBase
{
    private readonly TaskService _taskService; //Depency Injection - får ProjectService "insprøjtet" auto
    
    public TaskController(TaskService taskservice) // Constructor . bliver kaldt automatisk når controlleren laves
    { 
        _taskService = taskservice; //gemmer servicen, så den kan bruges i metoden
    }

    [HttpPost ("tasks")] //Opret nyt projekt , Blazor kalder denne når bruger trykker "Opret projekt"
    public async Task<IActionResult> Create(TaskModel model)
    {
        int pid = 1; // vi var nød til at kalde det et eller andet
        var project = new TaskModel
        {
            Name = model.Name,
            StartDate = model.StartDate,
            Deadline = model.Deadline,
            CompletionDate = model.CompletionDate,
            Status = model.Status,
            ProjectId = model.ProjectId,
        };
        
        await _taskService.CreateAsync(model, pid); // sender projektet til servicen;
        return Ok(model);
    }

    [HttpGet("project/{pid}")]
    public async Task<IActionResult> GetByProjectId(int pid)
    {
        var tasks = await _taskService.GetByProjectIdAsync(pid);
        return Ok(tasks);
    }


    [HttpGet("all")] // GET - hent alle projekter , fx til at vise projektlisten på forisden
    public async Task<IActionResult> GetAll()
    {
        var tasks = await _taskService.GetAllAsync(); //henter alle fra databasen
        return Ok(tasks);                           // statuskode 200 + sender listen tilbage
    }

    [HttpGet("{id}")] // hent et specifikt projekt med id = 5
    public async Task<IActionResult> GetbyId(int id)
    {
        var task = await _taskService.GetByIdAsync(id); // søger i databasen
        
        return task == null ? NotFound() : Ok(task); // hvis projekt ikke findes - 404 not found
                                                            // hvis det findes - 200 ok + projektet
                                                            // 201 betyder " jeg har lige lavet noget nyt og her er det fx nyt projekt"
    }
}