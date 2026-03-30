using TimeManagement.Api.Services.Roles;

namespace TimeManagement.Api.Endpoints.Roles;

public static class DeleteRole
{
    public static async Task<IResult> Handle(
        Guid id,
        IRoleService roleService,
        CancellationToken cancellationToken)
    {
        var result = await roleService.DeleteAsync(id, cancellationToken);

        return result.Status switch
        {
            RoleResultStatus.NoContent => Results.NoContent(),
            RoleResultStatus.NotFound => Results.NotFound(),
            RoleResultStatus.Conflict => Results.Conflict(result.Error),
            _ => Results.BadRequest(result.Error)
        };
    }
}
