using TimeManagement.Api.Services.Departments;

namespace TimeManagement.Api.Endpoints.Departments;

public static class GetDepartments
{
    public static async Task<IResult> Handle(
        IDepartmentService departmentService,
        CancellationToken cancellationToken)
    {
        var departments = await departmentService.GetAllAsync(cancellationToken);
        return Results.Ok(departments);
    }
}
