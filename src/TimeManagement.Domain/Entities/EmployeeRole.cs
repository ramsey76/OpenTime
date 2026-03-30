namespace TimeManagement.Domain.Entities;

public class EmployeeRole
{
    public Guid EmployeeId { get; set; }
    public Guid RoleId { get; set; }

    public Employee Employee { get; set; } = null!;
    public Role Role { get; set; } = null!;
}
