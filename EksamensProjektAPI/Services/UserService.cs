using EksamensProjekt.Models;

namespace EksamensProjektAPI.Services;

public class UserService
{
    private readonly UserRepo _repo;

    public UserService(UserRepo repo)
    {
        _repo = repo;
    }

    public async Task RegisterAsync(string username, string password, string email, bool isadmin)
    {
        var hash = BCrypt.Net.BCrypt.HashPassword(password);
        var user = new UserModel
        {
            Username = username,
            Password = hash,
            Email = email,
            IsAdmin = isadmin
  
        };
        await _repo.InsertAsync(user);
    }

    public async Task<UserModel?> LoginAsync(string username, string password)
    {
        var user = await _repo.GetByUsernameAsync(username);
        if (user == null) return null;

        if (BCrypt.Net.BCrypt.Verify(password, user.Password))
            return user;

        return null;
    }


}