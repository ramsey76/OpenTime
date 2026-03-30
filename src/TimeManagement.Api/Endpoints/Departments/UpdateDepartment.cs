using TimeManagement.Api.DTOs;
using TimeManagement.Api.Services.Departments;

namespace TimeManagement.Api.Endpoints.Departments;

public static class UpdateDepartment
{
    public static async Task<IResult> Handle(
        Guid id,
        UpdateDepartmentRequest request,
        IDepartmentService departmentService,
        CancellationToken cancellationToken)
    {
        var result = await departmentService.UpdateAsync(id, request, cancellationToken);

        return result.Status switch
        {
            DepartmentResultStatus.Ok => Results.Ok(result.Value),
            DepartmentResultStatus.NotFound => Results.NotFound(),
            DepartmentResultStatus.Conflict => Results.Conflict(result.Error),
            DepartmentResultStatus.UnprocessableEntity => Results.UnprocessableEntity(result.Error),
            _ => Results.BadRequest(result.Error)
        };
    }
}
