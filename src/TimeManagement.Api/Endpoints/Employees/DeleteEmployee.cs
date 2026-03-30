using TimeManagement.Api.Services.Employees;

namespace TimeManagement.Api.Endpoints.Employees;

public static class DeleteEmployee
{
    public static async Task<IResult> Handle(
        Guid id,
        IEmployeeService employeeService,
        CancellationToken cancellationToken)
    {
        var result = await employeeService.DeleteAsync(id, cancellationToken);

        return result.Status switch
        {
            EmployeeResultStatus.NoContent => Results.NoContent(),
            EmployeeResultStatus.NotFound => Results.NotFound(),
            _ => Results.BadRequest(result.Error)
        };
    }
}
