using EksamensProjekt.Models;

namespace EksamensProjektAPI.Services;

public class UserService
{
    private readonly UserRepo _repo; // Repository der håndterer databaseadgang for brugere

    public UserService(UserRepo repo) // Constructor der modtager UserRepo via Dependency Injection.
    {
        _repo = repo;
    }

    // Registrerer en ny bruger. Password hashes før det gemmes i databasen.
    public async Task RegisterAsync(string username, string password, string email, bool isadmin)
    {
        var hash = BCrypt.Net.BCrypt.HashPassword(password);// Hasher password for sikker opbevaring
        var user = new UserModel // Opretter nyt UserModel med hashed password
        {
            Username = username,
            Password = hash,
            Email = email,
            IsAdmin = isadmin
  
        };
        await _repo.InsertAsync(user); // Gemmer brugeren i databasen via repository
    }

    // Logger en bruger ind. Tjekker om brugeren findes og om password matcher hash. Retunerer UserModel hvis login er korrekt, ellers null
    public async Task<UserModel?> LoginAsync(string username, string password)
    {
        var user = await _repo.GetByUsernameAsync(username); // Henter bruger baseret på brugernavn
        if (user == null) return null;

        if (BCrypt.Net.BCrypt.Verify(password, user.Password)) // Sammenligner indtastet password med gemt hash
            return user; // Retunerer user hvis login er OK

        return null; // Login mislykkedes
    }

    // Henter alle brugere. Retunerer liste af brugere
    public async Task<List<UserModel>> GetAllAsync()
    {
        return await _repo.GetAllUsersAsync();
    }
}