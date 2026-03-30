namespace TimeManagement.Domain.Entities;

public class Employee
{
    public Guid Id { get; set; }
    public string ExternalId { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Guid DepartmentId { get; set; }

    public Department Department { get; set; } = null!;
    public ICollection<EmployeeRole> EmployeeRoles { get; set; } = new List<EmployeeRole>();
    public ICollection<ProjectAssignment> ProjectAssignments { get; set; } = new List<ProjectAssignment>();
}
