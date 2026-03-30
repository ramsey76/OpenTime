using TimeManagement.Api.Services.Employees;

namespace TimeManagement.Api.Endpoints.Employees;

public static class GetEmployeeById
{
    public static async Task<IResult> Handle(
        Guid id,
        IEmployeeService employeeService,
        CancellationToken cancellationToken)
    {
        var employee = await employeeService.GetByIdAsync(id, cancellationToken);
        return employee is null ? Results.NotFound() : Results.Ok(employee);
    }
}
