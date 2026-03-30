using TimeManagement.Api.DTOs;
using TimeManagement.Api.Services.Roles;

namespace TimeManagement.Api.Endpoints.Roles;

public static class UpdateRole
{
    public static async Task<IResult> Handle(
        Guid id,
        UpdateRoleRequest request,
        IRoleService roleService,
        CancellationToken cancellationToken)
    {
        var result = await roleService.UpdateAsync(id, request, cancellationToken);

        return result.Status switch
        {
            RoleResultStatus.Ok => Results.Ok(result.Value),
            RoleResultStatus.NotFound => Results.NotFound(),
            RoleResultStatus.Conflict => Results.Conflict(result.Error),
            _ => Results.BadRequest(result.Error)
        };
    }
}
