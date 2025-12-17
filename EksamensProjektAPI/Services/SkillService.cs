using EksamensProjekt.Models;
using EksamensProjektAPI.Repositories;

namespace EksamensProjektAPI.Services;

public class SkillService
{
    private readonly SkillRepo _repo = new();

    public async Task CreateAsync(SkillModel skill) // Metode til at oprette en ny skill, tager en skillModel som parameter
    {
        await _repo.InsertAsync(skill); // kalder InsertAsync fra repo
    }
    
    public async Task<List<SkillModel>> GetAllAsync() // Metode til at hente alle skills fra databasen
    {
        var skills = await _repo.GetAllAsync(); // kalder GetAllAsync fra repo
        return skills; // returnerer listen med skills tilbage
    }
}    