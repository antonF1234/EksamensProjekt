using EksamensProjekt.Models;

namespace EksamensProjektAPI.Services;

public class UserService
{
    private readonly UserRepo _repo;

    public UserService(UserRepo repo) // user repo is into the constructor
    {
        _repo = repo;
    }

    public async Task RegisterAsync(string username, string password, string email, bool isadmin)
    {
        var hash = BCrypt.Net.BCrypt.HashPassword(password);// uses password parameter from the function to hash the password
        var user = new UserModel // creates a new user, uses the password hash as the password, this happens client side
        {
            Username = username,
            Password = hash,
            Email = email,
            IsAdmin = isadmin
  
        };
        await _repo.InsertAsync(user); // use InsertAsync from UserRepo to insert the new user
    }

    public async Task<UserModel?> LoginAsync(string username, string password)
    {
        var user = await _repo.GetByUsernameAsync(username); // get user by username
        if (user == null) return null;

        if (BCrypt.Net.BCrypt.Verify(password, user.Password)) // uses database password hash and typed password to verify the login
            return user; // return user if login is OK

        return null; // else return null
    }

    public async Task<List<UserModel>> GetAllAsync()
    {
        return await _repo.GetAllUsersAsync();
    }
}