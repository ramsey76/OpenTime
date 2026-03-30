using TimeManagement.Api.Services.Employees;

namespace TimeManagement.Api.Endpoints.Employees;

public static class GetEmployees
{
    public static async Task<IResult> Handle(
        IEmployeeService employeeService,
        CancellationToken cancellationToken)
    {
        var employees = await employeeService.GetAllAsync(cancellationToken);
        return Results.Ok(employees);
    }
}
