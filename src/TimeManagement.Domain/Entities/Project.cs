namespace TimeManagement.Domain.Entities;

public class Project
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public ICollection<ProjectAssignment> ProjectAssignments { get; set; } = new List<ProjectAssignment>();
    public ICollection<TimeEntry> TimeEntries { get; set; } = new List<TimeEntry>();
}
