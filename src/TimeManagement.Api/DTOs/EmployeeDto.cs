namespace TimeManagement.Api.DTOs;

public sealed record EmployeeDto(Guid Id, string ExternalId, string FirstName, string LastName, string Email, Guid DepartmentId);
