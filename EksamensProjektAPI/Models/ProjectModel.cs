namespace EksamensProjekt.Models;

public class ProjectModel
{
    public int ProjectId { get; set; }
    public string Name { get; set; } = "";
    public string? Description { get; set; }
    public DateTime? Deadline { get; set; }
    public string? Status { get; set; }
}