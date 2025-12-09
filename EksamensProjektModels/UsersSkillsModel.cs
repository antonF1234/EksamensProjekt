namespace EksamensProjekt.Models;

public class UsersSkillsModel
{
    public int  UserSkillId { get; set; }
    
    public int UserId { get; set; }
    
    public int SkillId { get; set; }
    
    public string SkillName { get; set; } = "";
}