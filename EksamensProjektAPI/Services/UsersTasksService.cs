using EksamensProjekt.Models;
using EksamensProjektAPI.Repositories;

namespace EksamensProjektAPI.Services;

public class UsersTasksService
{
    private readonly UsersTasksRepo _repo = new();
    
    public async Task InsertUserTaskAsync(int taskId, int userId)
    {
        await _repo.InsertAsync(taskId, userId);
    }

    public async Task<List<UsersTasksModel>> GetAllUserTasksAsync(int userId)
    {
        return await _repo.GetAllUserTasksAsync(userId);
    }
}