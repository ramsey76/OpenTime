using TimeManagement.Api.DTOs;
using TimeManagement.Api.Services.Employees;

namespace TimeManagement.Api.Endpoints.Employees;

public static class UpdateEmployee
{
    public static async Task<IResult> Handle(
        Guid id,
        UpdateEmployeeRequest request,
        IEmployeeService employeeService,
        CancellationToken cancellationToken)
    {
        var result = await employeeService.UpdateAsync(id, request, cancellationToken);

        return result.Status switch
        {
            EmployeeResultStatus.Ok => Results.Ok(result.Value),
            EmployeeResultStatus.NotFound => Results.NotFound(),
            EmployeeResultStatus.UnprocessableEntity => Results.UnprocessableEntity(result.Error),
            _ => Results.BadRequest(result.Error)
        };
    }
}
