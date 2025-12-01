namespace EksamensProjekt.Models;

public class AuthState
{
    public string? CurrentUser { get; set; }
    public int? UserId { get; set; }
    
    public bool IsAdmin { get; set; }
}