using TimeManagement.Api.DTOs;

namespace TimeManagement.Api.Services.Employees;

public sealed record EmployeeResult(bool IsSuccess, EmployeeDto? Value, string? Error, EmployeeResultStatus Status)
{
    public static EmployeeResult Success(EmployeeDto value) =>
        new(true, value, null, EmployeeResultStatus.Ok);

    public static EmployeeResult NotFound() =>
        new(false, null, "Employee not found.", EmployeeResultStatus.NotFound);

    public static EmployeeResult Conflict(string error) =>
        new(false, null, error, EmployeeResultStatus.Conflict);

    public static EmployeeResult NoContent() =>
        new(true, null, null, EmployeeResultStatus.NoContent);

    public static EmployeeResult UnprocessableEntity(string error) =>
        new(false, null, error, EmployeeResultStatus.UnprocessableEntity);
}

public enum EmployeeResultStatus
{
    Ok,
    NoContent,
    NotFound,
    Conflict,
    UnprocessableEntity
}
