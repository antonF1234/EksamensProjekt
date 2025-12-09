using EksamensProjektAPI.Repositories;

namespace EksamensProjektAPI.Services;

public class UsersSkillsService
{
    private readonly UsersSkillsRepo _repo = new();
    
    public async Task InsertUserSkillAsync(int skillId, int userId)
    {
        await _repo.InsertAsync(skillId, userId);
    }
}