using TimeManagement.Api.Services.Departments;

namespace TimeManagement.Api.Endpoints.Departments;

public static class GetDepartmentById
{
    public static async Task<IResult> Handle(
        Guid id,
        IDepartmentService departmentService,
        CancellationToken cancellationToken)
    {
        var department = await departmentService.GetByIdAsync(id, cancellationToken);
        return department is null ? Results.NotFound() : Results.Ok(department);
    }
}
