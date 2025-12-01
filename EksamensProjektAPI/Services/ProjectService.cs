using EksamensProjekt.Models;

namespace EksamensProjektAPI.Services;

public class ProjectService
{
    private readonly ProjectRepo _repo = new();
    
    public async Task CreateAsync(ProjectModel p)
        => await _repo.InsertAsync(p);
    
    public async Task<List<ProjectModel>> GetAllAsync()
        => await _repo.GetAllAsync();
    
    public async Task<ProjectModel?> GetByIdAsync(int id)
        => await _repo.GetByIdAsync(id);
}