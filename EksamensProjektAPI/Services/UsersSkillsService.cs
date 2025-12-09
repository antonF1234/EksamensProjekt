using EksamensProjekt.Models;
using EksamensProjektAPI.Repositories;

namespace EksamensProjektAPI.Services;

public class UsersSkillsService
{
    private readonly UsersSkillsRepo _repo = new();
    
    public async Task InsertUserSkillAsync(int skillId, int userId)
    {
        await _repo.InsertAsync(skillId, userId);
    }
    
    public async Task<List<UsersSkillsModel>> GetAllUserSkillsAsync(int userId)
    {
        return await _repo.GetAllUserSkillsAsync(userId);
    }
    
    public async Task DeleteUserSkillAsync(int userSkillId)
    {
        await _repo.DeleteAsync(userSkillId);
    }
}