using EksamensProjekt.Models;

namespace EksamensProjektAPI.Services;

public class ProjectService
{
    private readonly ProjectRepo _repo = new();
    
    public async Task CreateAsync(ProjectModel p, int uid) // Opretter et nyt projekt og knytter det til en bruger.
        => await _repo.InsertAsync(p, uid); // p = Projektet der skal oprettes, uid = ID på brugeren der ejer projektet
    
    public async Task<List<ProjectModel>> GetAllAsync() // Henter alle projekter fra databasen.
        => await _repo.GetAllAsync(); // retunere liste af projekter
    
    public async Task<ProjectModel?> GetByIdAsync(int id) // Henter et projekt baseret på dets ID.
        => await _repo.GetByIdAsync(id); // retunere projektet hvis det findes, ellers null
    
    public async Task DeleteAsync(ProjectModel p) // Sletter et eksisterende projekt.
        => await _repo.DeleteAsync(p); // p = Projektet der skal slettes
    
    public async Task UpdateAsync(ProjectModel p) // Opdaterer et eksisterende projekt.
    => await _repo.UpdateAsync(p); // p = Projektet med opdaterede værdier
}