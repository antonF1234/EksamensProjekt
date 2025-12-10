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

    [HttpPost("createtask/{projectId}")]
    public async Task<IActionResult> Create(int projectId, [FromBody] TaskModel model)
    {
        if (model == null) return BadRequest("Task is null");
        await _taskService.CreateAsync(model, projectId);
        return Ok(model);
    }





    [HttpGet("project/{pid}")]
    public async Task<IActionResult> GetByProjectId(int pid)
    {
        var tasks = await _taskService.GetByProjectIdAsync(pid);
        return Ok(tasks);
    }
    
    [HttpGet("all")] 
    public async Task<IActionResult> GetAll()
    {
        var tasks = await _taskService.GetAllAsync(); //henter alle fra databasen
        return Ok(tasks);                           // statuskode 200 + sender listen tilbage
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetbyId(int id)
    {
        var task = await _taskService.GetByIdAsync(id); // søger i databasen
        
        return task == null ? NotFound() : Ok(task); // hvis projekt ikke findes - 404 not found
                                                            // hvis det findes - 200 ok + projektet
                                                            // 201 betyder " jeg har lige lavet noget nyt og her er det fx nyt projekt"
    }
    
    [HttpPut("updatestatus/{id}/{status}")]
    public async Task<IActionResult> UpdateStatus(int id, string status)
    {
        var updatedTask = await _taskService.UpdateAsync(id, status);
        if (updatedTask == null) return NotFound();

        return Ok(updatedTask);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _taskService.DeleteAsync(id);
        return Ok();
    }
    
    [HttpPut("update/{id}")]
    public async Task<IActionResult> Update(int id, TaskModel model)
    {
        var task = await _taskService.GetByIdAsync(id);
        if (task == null) 
        {
            Console.WriteLine($"Task med id {id} findes ikke!");
            return NotFound();
        }


        task.Name = model.Name;
        task.StartDate = model.StartDate;
        task.Deadline = model.Deadline;
        task.CompletionDate = model.CompletionDate;
        task.Status = model.Status;
        task.ProjectId = model.ProjectId;
        
        

        await _taskService.UpdateAsync(task);
        return Ok();
    }
}
