using TimeManagement.Api.DTOs;

namespace TimeManagement.Api.Endpoints.Roles;

public sealed class RoleEndpoints : EndPoints
{
    public override void MapEndpoints(WebApplication app)
    {
        var group = app.MapGroup("/api/roles").WithTags("Roles");

        group.MapGet("/", GetRoles.Handle)
            .WithName("GetRoles")
            .Produces<List<RoleDto>>(StatusCodes.Status200OK);

        group.MapGet("/{id:guid}", GetRoleById.Handle)
            .WithName("GetRoleById")
            .Produces<RoleDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", CreateRole.Handle)
            .WithName("CreateRole")
            .Produces<RoleDto>(StatusCodes.Status201Created)
            .Produces<string>(StatusCodes.Status409Conflict)
            .Produces(StatusCodes.Status400BadRequest);

        group.MapPut("/{id:guid}", UpdateRole.Handle)
            .WithName("UpdateRole")
            .Produces<RoleDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces<string>(StatusCodes.Status409Conflict)
            .Produces(StatusCodes.Status400BadRequest);

        group.MapDelete("/{id:guid}", DeleteRole.Handle)
            .WithName("DeleteRole")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces<string>(StatusCodes.Status409Conflict);
    }
}
