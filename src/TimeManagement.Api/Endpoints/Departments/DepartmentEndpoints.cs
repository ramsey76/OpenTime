using TimeManagement.Api.DTOs;

namespace TimeManagement.Api.Endpoints.Departments;

public sealed class DepartmentEndpoints : EndPoints
{
    public override void MapEndpoints(WebApplication app)
    {
        var group = app.MapGroup("/api/departments").WithTags("Departments");

        group.MapGet("/", GetDepartments.Handle)
            .WithName("GetDepartments")
            .Produces<List<DepartmentDto>>(StatusCodes.Status200OK);

        group.MapGet("/{id:guid}", GetDepartmentById.Handle)
            .WithName("GetDepartmentById")
            .Produces<DepartmentDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", CreateDepartment.Handle)
            .WithName("CreateDepartment")
            .Produces<DepartmentDto>(StatusCodes.Status201Created)
            .Produces<string>(StatusCodes.Status409Conflict)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status422UnprocessableEntity);

        group.MapPut("/{id:guid}", UpdateDepartment.Handle)
            .WithName("UpdateDepartment")
            .Produces<DepartmentDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces<string>(StatusCodes.Status409Conflict)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status422UnprocessableEntity);

        group.MapDelete("/{id:guid}", DeleteDepartment.Handle)
            .WithName("DeleteDepartment")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces<string>(StatusCodes.Status409Conflict);
    }
}
