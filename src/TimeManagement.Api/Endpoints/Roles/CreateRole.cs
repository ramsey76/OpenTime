using TimeManagement.Api.DTOs;
using TimeManagement.Api.Services.Roles;

namespace TimeManagement.Api.Endpoints.Roles;

public static class CreateRole
{
    public static async Task<IResult> Handle(
        CreateRoleRequest request,
        IRoleService roleService,
        CancellationToken cancellationToken)
    {
        var result = await roleService.CreateAsync(request, cancellationToken);

        return result.Status switch
        {
            RoleResultStatus.Ok => Results.Created($"/api/roles/{result.Value!.Id}", result.Value),
            RoleResultStatus.Conflict => Results.Conflict(result.Error),
            _ => Results.BadRequest(result.Error)
        };
    }
}
