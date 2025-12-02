using EksamensProjekt.Models;
namespace EksamensProjektAPI.Services;

public class TaskService
{
    private readonly TaskRepo _repo = new();

    public async Task CreateAsync(TaskModel task, int projectId)
    {
        task.ProjectId = projectId; 
        await _repo.InsertAsync(task);
    }
    
    public async Task<List<TaskModel>> GetAllAsync()
        => await _repo.GetAllAsync();

    public async Task<List<TaskModel>> GetByProjectIdAsync(int projectId)
        => await _repo.GetByProjectIdAsync(pid: projectId);
    
    public async Task<TaskModel?> GetByIdAsync(int id)
        => await _repo.GetByIdAsync(id);
}