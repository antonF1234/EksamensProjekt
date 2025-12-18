using EksamensProjekt.Models;
using EksamensProjektAPI.Repositories;

namespace EksamensProjektAPI.Services;

public class UsersTasksService
{
    private readonly UsersTasksRepo _repo = new(); // laver en readonly reference til usertasksrepo
    
    // Tilføjer en bruger til en opgave (fx når admin vælger "Tilføj bruger til opgave" i Blazor)
    public async Task InsertUserTaskAsync(int taskId, int userId) 
    {
        // Sender taskId og userId videre til repo'en, som laver en ny række i users_tasks-tabellen
        await _repo.InsertAsync(taskId, userId);
    }

    // Henter alle opgave-tilknytninger for en bestemt bruger
    public async Task<List<UsersTasksModel>> GetAllUserTasksAsync(int userId)
    {
        // Kalder repo'en og får en liste tilbage med alle opgaver brugeren er tilknyttet
        return await _repo.GetAllUserTasksAsync(userId);
    }

    // Henter alle brugere der er tilknyttet en bestemt opgave 
    public async Task<List<UserModel>> GetAllUsersOnTask(int taskId)
    {
        // Repo'en finder alle brugere på den opgave og sender deres data tilbage
        return await _repo.GetUsersByTaskIdAsync(taskId);
    }

    // Fjerner en bruger fra en opgave
    public async Task DeleteUserTaskAsync(int taskId, int userId)
    {
        await _repo.DelUserFromTask(taskId, userId);
    }

}