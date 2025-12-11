using EksamensProjekt.Models;
using EksamensProjektAPI.Repositories;

namespace EksamensProjektAPI.Services;

public class TasksSkillsService
{
    private readonly TasksSkillsRepo _repo = new();
    
    public async Task InsertTaskSkillAsync(int taskId, int skillId)
    {
        await _repo.InsertTaskSkillAsync(taskId, skillId);
    }

    public async Task DeleteTaskSkill(int taskId, int skillId)
    {
        await _repo.DeleteTaskSkillAsync(taskId, skillId);
    }
    
    public async Task<List<SkillModel>> GetAllSkillsForTaskAsync(int taskId)
    {
        return await _repo.GetSkillsForTaskAsync(taskId);
    }
}