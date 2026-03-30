using TimeManagement.Api.DTOs;
using TimeManagement.Api.Services.Employees;

namespace TimeManagement.Api.Endpoints.Employees;

public static class CreateEmployee
{
    public static async Task<IResult> Handle(
        CreateEmployeeRequest request,
        IEmployeeService employeeService,
        CancellationToken cancellationToken)
    {
        var result = await employeeService.CreateAsync(request, cancellationToken);

        return result.Status switch
        {
            EmployeeResultStatus.Ok => Results.Created($"/api/employees/{result.Value!.Id}", result.Value),
            EmployeeResultStatus.Conflict => Results.Conflict(result.Error),
            EmployeeResultStatus.UnprocessableEntity => Results.UnprocessableEntity(result.Error),
            _ => Results.BadRequest(result.Error)
        };
    }
}
