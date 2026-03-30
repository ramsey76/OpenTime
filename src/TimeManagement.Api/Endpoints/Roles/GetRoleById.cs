using TimeManagement.Api.Services.Roles;

namespace TimeManagement.Api.Endpoints.Roles;

public static class GetRoleById
{
    public static async Task<IResult> Handle(
        Guid id,
        IRoleService roleService,
        CancellationToken cancellationToken)
    {
        var role = await roleService.GetByIdAsync(id, cancellationToken);
        return role is null ? Results.NotFound() : Results.Ok(role);
    }
}
