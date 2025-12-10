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

    public async Task<UsersTasksModel?> UpdateAsync(int taskId, string status)
    {
        // Update the status directly via repo
        await _repo.UpdateTaskAsync(taskId, status);

        // Fetch the updated task to return as UsersTasksModel
        var task = await _repo.GetByIdAsync(taskId);
        if (task == null) return null;

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

    public async Task DeleteAsync(int taskId)
    {
        await _repo.DeleteTaskAsync(taskId);
    }
    public async Task UpdateAsync(TaskModel t)
        => await _repo.UpdateAsync(t);

}