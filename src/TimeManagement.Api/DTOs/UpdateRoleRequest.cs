using System.ComponentModel.DataAnnotations;

namespace TimeManagement.Api.DTOs;

public sealed record UpdateRoleRequest(
    [Required][MaxLength(50)] string Name,
    string? Description);
