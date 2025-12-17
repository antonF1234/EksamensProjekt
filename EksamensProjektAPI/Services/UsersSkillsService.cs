using EksamensProjekt.Models;
using EksamensProjektAPI.Repositories;

namespace EksamensProjektAPI.Services;

public class UsersSkillsService
{
    private readonly UsersSkillsRepo _repo = new(); // instansierer repo objektet som privat bruges af alle metoder
    
    public async Task InsertUserSkillAsync(int skillId, int userId) // metode til at oprette en ny users_skills relation, tager skillId og userId som parameter
    {
        await _repo.InsertAsync(skillId, userId); // kalder InsertAsync fra repo med skillId og userId
    }
    
    public async Task<List<UsersSkillsModel>> GetAllUserSkillsAsync(int userId) // metode til at hente alle skills for en bruger
    {
        return await _repo.GetAllUserSkillsAsync(userId); // kalder GetAllUserSkillsAsync fra repo med userId
    }
    
    public async Task DeleteUserSkillAsync(int skillId, int userId) // metode til at slette en users_skills relation
    {
        await _repo.DeleteAsync(skillId, userId); // kalder DeleteAsync fra repo med skillId og userId
    }
}