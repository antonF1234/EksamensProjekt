namespace EksamensProjekt.Models;

public class TimeRecordingModel
{
    public int TimeRecordId { get; set; }
    public int UserId { get; set; }
    public int TaskId { get; set; }
    public string UserName { get; set; } = "";
    public string TaskName { get; set; } = "";
    
    public DateTime? StartTime { get; set; }
    
    public DateTime? EndTime { get; set; }
    
    public int SumOfTimeS{ get; set; }
}