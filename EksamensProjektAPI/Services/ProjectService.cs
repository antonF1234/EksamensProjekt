using EksamensProjekt.Models;

namespace EksamensProjektAPI.Services;

public class ProjectService
{
    private readonly ProjectRepo _repo = new();
    
    public async Task CreateAsync(ProjectModel p, int uid)
        => await _repo.InsertAsync(p, uid);
    
    public async Task<List<ProjectModel>> GetAllAsync()
        => await _repo.GetAllAsync();
    
    public async Task<ProjectModel?> GetByIdAsync(int id)
        => await _repo.GetByIdAsync(id);
    
    public async Task DeleteAsync(ProjectModel p)
        => await _repo.DeleteAsync(p);
    
    public async Task UpdateAsync(ProjectModel p)
    => await _repo.UpdateAsync(p);
}