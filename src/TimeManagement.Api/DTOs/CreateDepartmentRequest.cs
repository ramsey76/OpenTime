using System.ComponentModel.DataAnnotations;

namespace TimeManagement.Api.DTOs;

public sealed record CreateDepartmentRequest(
    [Required][MaxLength(100)] string Name,
    [Required][MaxLength(10)] string Code,
    Guid? ParentDepartmentId = null);
