using TimeManagement.Api.DTOs;
using TimeManagement.Api.Services.Departments;

namespace TimeManagement.Api.Endpoints.Departments;

public static class CreateDepartment
{
    public static async Task<IResult> Handle(
        CreateDepartmentRequest request,
        IDepartmentService departmentService,
        CancellationToken cancellationToken)
    {
        var result = await departmentService.CreateAsync(request, cancellationToken);

        return result.Status switch
        {
            DepartmentResultStatus.Ok => Results.Created($"/api/departments/{result.Value!.Id}", result.Value),
            DepartmentResultStatus.Conflict => Results.Conflict(result.Error),
            DepartmentResultStatus.UnprocessableEntity => Results.UnprocessableEntity(result.Error),
            _ => Results.BadRequest(result.Error)
        };
    }
}
