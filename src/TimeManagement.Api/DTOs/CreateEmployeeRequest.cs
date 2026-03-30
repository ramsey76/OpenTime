using System.ComponentModel.DataAnnotations;

namespace TimeManagement.Api.DTOs;

public sealed record CreateEmployeeRequest(
    [Required][MaxLength(128)] string ExternalId,
    [Required][MaxLength(100)] string FirstName,
    [Required][MaxLength(100)] string LastName,
    [Required][MaxLength(256)] string Email,
    [Required] Guid DepartmentId);
