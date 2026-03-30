using TimeManagement.Api.DTOs;

namespace TimeManagement.Api.Endpoints.Employees;

public sealed class EmployeeEndpoints : EndPoints
{
    public override void MapEndpoints(WebApplication app)
    {
        var group = app.MapGroup("/api/employees").WithTags("Employees");

        group.MapGet("/", GetEmployees.Handle)
            .WithName("GetEmployees")
            .Produces<List<EmployeeDto>>(StatusCodes.Status200OK);

        group.MapGet("/{id:guid}", GetEmployeeById.Handle)
            .WithName("GetEmployeeById")
            .Produces<EmployeeDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        group.MapPost("/", CreateEmployee.Handle)
            .WithName("CreateEmployee")
            .Produces<EmployeeDto>(StatusCodes.Status201Created)
            .Produces<string>(StatusCodes.Status409Conflict)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status422UnprocessableEntity);

        group.MapPut("/{id:guid}", UpdateEmployee.Handle)
            .WithName("UpdateEmployee")
            .Produces<EmployeeDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status422UnprocessableEntity);

        group.MapDelete("/{id:guid}", DeleteEmployee.Handle)
            .WithName("DeleteEmployee")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound);
    }
}
