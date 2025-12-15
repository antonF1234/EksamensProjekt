using EksamensProjekt.Models;
using EksamensProjektAPI.Services;

public class AuthService
{
    private readonly AuthRepo _repo;

    public AuthService(AuthRepo repo)
    {
        _repo = repo;
    }

    public async Task<UserModel?> LoginAsync(string username, string password)
    {
        var user = await _repo.GetByUsernameAsync(username);
        if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            return user;

        return null;
    }

    public async Task RegisterAsync(string username, string password, string email, bool isAdmin)
    {
        var hashed = BCrypt.Net.BCrypt.HashPassword(password);
        var user = new UserModel
        {
            Username = username,
            Password = hashed,
            Email = email,
            IsAdmin = isAdmin
        };
        await _repo.InsertAsync(user);
    }
}