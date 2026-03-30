namespace TimeManagement.Domain.Entities;

public class TimeEntry
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public Guid ProjectId { get; set; }
    public DateOnly Date { get; set; }
    public decimal Hours { get; set; }
    public string Description { get; set; } = string.Empty;

    public Employee Employee { get; set; } = null!;
    public Project Project { get; set; } = null!;
}
