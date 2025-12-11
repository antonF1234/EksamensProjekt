namespace EksamensProjekt.Models;

public class UsersTasksModel
{
    public int UserTaskId { get; set; }
    public int UserId { get; set; }
    public int TaskId { get; set; }
    
    public int ProjectId { get; set; }
    public string TaskName { get; set; } = "";
    public string UserName { get; set; } = "";
    
    public DateTime? StartDate { get; set; }
    
    public DateTime? Deadline { get; set; }
    
    public DateTime? CompletionDate { get; set; }
    
    public string Status { get; set; } = "";
}