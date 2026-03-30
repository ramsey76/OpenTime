using TimeManagement.Api.Services.Departments;

namespace TimeManagement.Api.Endpoints.Departments;

public static class DeleteDepartment
{
    public static async Task<IResult> Handle(
        Guid id,
        IDepartmentService departmentService,
        CancellationToken cancellationToken)
    {
        var result = await departmentService.DeleteAsync(id, cancellationToken);

        return result.Status switch
        {
            DepartmentResultStatus.NoContent => Results.NoContent(),
            DepartmentResultStatus.NotFound => Results.NotFound(),
            DepartmentResultStatus.Conflict => Results.Conflict(result.Error),
            _ => Results.BadRequest(result.Error)
        };
    }
}
