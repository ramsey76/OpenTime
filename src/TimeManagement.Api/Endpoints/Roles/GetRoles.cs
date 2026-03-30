using TimeManagement.Api.DTOs;
using TimeManagement.Api.Services.Roles;

namespace TimeManagement.Api.Endpoints.Roles;

public static class GetRoles
{
    public static async Task<IResult> Handle(
        IRoleService roleService,
        CancellationToken cancellationToken)
    {
        var roles = await roleService.GetAllAsync(cancellationToken);
        return Results.Ok(roles);
    }
}
