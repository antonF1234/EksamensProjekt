namespace EksamensProjekt.Models;

public class UserModel
{
    public int UserId { get; set; }
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string Email { get; set; } = "";
    public bool IsAdmin { get; set; }
}