using TimeManagement.Api.DTOs;

namespace TimeManagement.Api.Services.Departments;

public sealed record DepartmentResult(bool IsSuccess, DepartmentDto? Value, string? Error, DepartmentResultStatus Status)
{
    public static DepartmentResult Success(DepartmentDto value) =>
        new(true, value, null, DepartmentResultStatus.Ok);

    public static DepartmentResult NotFound() =>
        new(false, null, "Department not found.", DepartmentResultStatus.NotFound);

    public static DepartmentResult Conflict(string error) =>
        new(false, null, error, DepartmentResultStatus.Conflict);

    public static DepartmentResult NoContent() =>
        new(true, null, null, DepartmentResultStatus.NoContent);

    public static DepartmentResult UnprocessableEntity(string error) =>
        new(false, null, error, DepartmentResultStatus.UnprocessableEntity);
}

public enum DepartmentResultStatus
{
    Ok,
    NoContent,
    NotFound,
    Conflict,
    UnprocessableEntity
}
