using EksamensProjekt.Models;
using EksamensProjektAPI.Repositories;

namespace EksamensProjektAPI.Services;

public class SkillService
{
    private readonly SkillRepo _repo = new();

    public async Task CreateAsync(SkillModel skill)
    {
        await _repo.InsertAsync(skill);
    }
    
    public async Task<List<SkillModel>> GetAllAsync()
    {
        var skills = await _repo.GetAllAsync();
        return skills;
    }
}    