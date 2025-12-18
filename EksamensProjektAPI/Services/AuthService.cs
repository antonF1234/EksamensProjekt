using EksamensProjekt.Models;
using EksamensProjektAPI.Services;

public class AuthService
{
    private readonly AuthRepo _repo; //  AuthRepo via dependency injection

    public AuthService(AuthRepo repo) // Constructor – ASP.NET giver os automatisk en AuthRepo, når klassen laves
    {
        _repo = repo; // Gemmer den injicerede AuthRepo, så vi kan bruge den til databasekald
    }

    public async Task<UserModel?> LoginAsync(string username, string password) // Tjekker om brugernavn og kodeord passer – bruges når nogen logger ind
    {
        var user = await _repo.GetByUsernameAsync(username); // Først henter vi brugeren fra databasen ud fra brugernavnet
        
        // Returnerer null hvis brugeren ikke findes eller kodeordet ikke matcher
        if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password)) //Hvis brugeren findes OG kodeordet passer, så log ind.
            
            // BCrypt.Verify tjekker om det kodeord brugeren skrev, er det samme som det hashed kodeord i databasen
            // Hvis ja så lykkedes login og vi sender hele bruger-objektet tilbage
            return user;

        return null; // Hvis noget er forkert → login mislykkes
    }

    // Opretter en ny bruger – bruges fra register-endpointet
    public async Task RegisterAsync(string username, string password, string email, bool isAdmin)
    {
        // Først hasher vi kodeordet med BCrypt, så det aldrig gemmes i klar tekst
        // sikkerhed, ingen kan se det rigtige kodeord
        var hashed = BCrypt.Net.BCrypt.HashPassword(password);
       
        var user = new UserModel // Laver et nyt UserModel-objekt med de oplysninger vi har fået
        {
            Username = username,
            Password = hashed,
            Email = email,
            IsAdmin = isAdmin
        };
        await _repo.InsertAsync(user); // Sender den nye bruger videre til repo'en, som gemmer den i databasen
    }
}