namespace EksamensProjekt.Models;

public class TaskModel
{
    public int TaskId { get; set; }
    public string Name { get; set; } = "";
    public DateTime? StartDate { get; set; }
    public DateTime? Deadline { get; set; }
    public DateTime? CompletionDate { get; set; }
    public string? Status { get; set; }

    public int ProjectId { get; set; }   // FK
    
    public int? AssignedToUserId { get; set; }
}