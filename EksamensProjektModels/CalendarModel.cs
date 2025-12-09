// Models/CalendarEventModel.cs
namespace EksamensProjekt.Models
{
    public class CalendarModel
    {
        public string Title { get; set; } = "";
        public DateTime Date { get; set; }
        public string Type { get; set; } = "";        // "Projekt" eller "Opgave"
        public string AssignedTo { get; set; } = ""; // f.eks. "Anders" eller "gruppe"
        public bool IsOverdue { get; set; }
    }
}