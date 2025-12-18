using EksamensProjekt.Models;   

namespace EksamensProjektAPI.Services;

public class TaskService
{
    // Repository-klassen gør alt det direkte arbejde med databasen
    private readonly TaskRepo _repo = new();

    // Opretter en ny opgave og tilknytter den til det rigtige projekt
    public async Task CreateAsync(TaskModel task, int projectId)
    {
        task.ProjectId = projectId;          // Sætter projekt-ID på opgaven
        await _repo.InsertAsync(task);       // Gemmer opgaven i databasen
    }

    // Henter alle opgaver fra databasen
    public async Task<List<TaskModel>> GetAllAsync() 
        => await _repo.GetAllAsync();

    // Henter alle opgaver for et bestemt projekt
    public async Task<List<TaskModel>> GetByProjectIdAsync(int projectId) 
        => await _repo.GetByProjectIdAsync(pid: projectId);

    // Henter én enkelt opgave ud fra dens ID (returnerer null hvis den ikke findes)
    public async Task<TaskModel?> GetByIdAsync(int id) 
        => await _repo.GetByIdAsync(id);

    // Opdaterer kun status på en opgave (fx fra "Igang" til "Færdig")
    public async Task<UsersTasksModel?> UpdateAsync(int taskId, string status)
    {
        await _repo.UpdateTaskStatusAsync(taskId, status);  // Opdaterer status i databasen

        var task = await _repo.GetByIdAsync(taskId);        // Henter den opdaterede opgave
        if (task == null) return null;                     // Hvis den ikke findes → returner null

        // Laver et mindre objekt (UsersTasksModel) med kun de felter, frontend har brug for
        return new UsersTasksModel
        {
            TaskId = task.TaskId,
            TaskName = task.Name,
            Status = task.Status,
            StartDate = task.StartDate,
            Deadline = task.Deadline,
            CompletionDate = task.CompletionDate
        };
    }

    // Sletter en opgave permanent ud fra ID
    public async Task DeleteAsync(int taskId)
    {
        await _repo.DeleteTaskAsync(taskId);
    }

    // Opdaterer alle felter på en eksisterende opgave (bruges fra update-modalen)
    public async Task UpdateAsync(TaskModel t) 
        => await _repo.UpdateAsync(t);
}