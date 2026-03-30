namespace TimeManagement.Domain.Entities;

public class ProjectAssignment
{
    public Guid EmployeeId { get; set; }
    public Guid ProjectId { get; set; }

    public Employee Employee { get; set; } = null!;
    public Project Project { get; set; } = null!;
}
