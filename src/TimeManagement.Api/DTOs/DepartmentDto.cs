namespace TimeManagement.Api.DTOs;

public sealed record DepartmentDto(Guid Id, string Name, string Code, Guid? ParentDepartmentId);
