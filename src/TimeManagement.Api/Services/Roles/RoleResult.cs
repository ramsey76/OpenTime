using TimeManagement.Api.DTOs;

namespace TimeManagement.Api.Services.Roles;

public sealed record RoleResult(bool IsSuccess, RoleDto? Value, string? Error, RoleResultStatus Status)
{
    public static RoleResult Success(RoleDto value) =>
        new(true, value, null, RoleResultStatus.Ok);

    public static RoleResult NotFound() =>
        new(false, null, "Role not found.", RoleResultStatus.NotFound);

    public static RoleResult Conflict(string error) =>
        new(false, null, error, RoleResultStatus.Conflict);

    public static RoleResult NoContent() =>
        new(true, null, null, RoleResultStatus.NoContent);
}

public enum RoleResultStatus
{
    Ok,
    NoContent,
    NotFound,
    Conflict
}
