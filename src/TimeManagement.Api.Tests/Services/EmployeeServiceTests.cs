using Microsoft.EntityFrameworkCore;
using TimeManagement.Api.DTOs;
using TimeManagement.Api.Services.Employees;
using TimeManagement.Domain.Entities;
using TimeManagement.Infrastructure.Database;

namespace TimeManagement.Api.Tests.Services;

public class EmployeeServiceTests
{
    private static AppDbContext CreateContext() =>
        new(new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    private static Department SeedDepartment(AppDbContext context)
    {
        var dept = new Department { Id = Guid.NewGuid(), Name = "Engineering", Code = "ENG" };
        context.Departments.Add(dept);
        context.SaveChanges();
        return dept;
    }

    private static Employee SeedEmployee(AppDbContext context, Guid departmentId, string externalId = "EXT-001")
    {
        var emp = new Employee
        {
            Id = Guid.NewGuid(),
            ExternalId = externalId,
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane.doe@example.com",
            DepartmentId = departmentId
        };
        context.Employees.Add(emp);
        context.SaveChanges();
        return emp;
    }

    // ── GetAllAsync ──────────────────────────────────────────────────────────

    [Fact]
    public async Task GetAllAsync_WithEmployees_ReturnsAllEmployeeDtos()
    {
        using var context = CreateContext();
        var dept = SeedDepartment(context);
        context.Employees.AddRange(
            new Employee { Id = Guid.NewGuid(), ExternalId = "E1", FirstName = "Alice", LastName = "Smith", Email = "alice@example.com", DepartmentId = dept.Id },
            new Employee { Id = Guid.NewGuid(), ExternalId = "E2", FirstName = "Bob", LastName = "Jones", Email = "bob@example.com", DepartmentId = dept.Id });
        await context.SaveChangesAsync();

        var service = new EmployeeService(context);
        var result = await service.GetAllAsync(CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.Contains(result, e => e.ExternalId == "E1" && e.FirstName == "Alice");
        Assert.Contains(result, e => e.ExternalId == "E2" && e.FirstName == "Bob");
    }

    [Fact]
    public async Task GetAllAsync_Empty_ReturnsEmptyList()
    {
        using var context = CreateContext();
        var service = new EmployeeService(context);

        var result = await service.GetAllAsync(CancellationToken.None);

        Assert.Empty(result);
    }

    // ── GetByIdAsync ─────────────────────────────────────────────────────────

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsEmployeeDto()
    {
        using var context = CreateContext();
        var dept = SeedDepartment(context);
        var emp = SeedEmployee(context, dept.Id);

        var service = new EmployeeService(context);
        var result = await service.GetByIdAsync(emp.Id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(emp.Id, result.Id);
        Assert.Equal("EXT-001", result.ExternalId);
        Assert.Equal("Jane", result.FirstName);
        Assert.Equal("Doe", result.LastName);
    }

    [Fact]
    public async Task GetByIdAsync_UnknownId_ReturnsNull()
    {
        using var context = CreateContext();
        var service = new EmployeeService(context);

        var result = await service.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.Null(result);
    }

    // ── CreateAsync ──────────────────────────────────────────────────────────

    [Fact]
    public async Task CreateAsync_ValidRequest_ReturnsSuccess()
    {
        using var context = CreateContext();
        var dept = SeedDepartment(context);
        var service = new EmployeeService(context);

        var result = await service.CreateAsync(
            new CreateEmployeeRequest("EXT-100", "John", "Smith", "john@example.com", dept.Id),
            CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("EXT-100", result.Value.ExternalId);
        Assert.Equal("John", result.Value.FirstName);
        Assert.NotEqual(Guid.Empty, result.Value.Id);
    }

    [Fact]
    public async Task CreateAsync_ValidRequest_PersistsToDatabase()
    {
        using var context = CreateContext();
        var dept = SeedDepartment(context);
        var service = new EmployeeService(context);

        await service.CreateAsync(
            new CreateEmployeeRequest("EXT-100", "John", "Smith", "john@example.com", dept.Id),
            CancellationToken.None);

        var saved = await context.Employees.FirstOrDefaultAsync(e => e.ExternalId == "EXT-100");
        Assert.NotNull(saved);
        Assert.Equal("John", saved.FirstName);
    }

    [Fact]
    public async Task CreateAsync_DuplicateExternalId_ReturnsConflict()
    {
        using var context = CreateContext();
        var dept = SeedDepartment(context);
        SeedEmployee(context, dept.Id, "EXT-001");
        var service = new EmployeeService(context);

        var result = await service.CreateAsync(
            new CreateEmployeeRequest("EXT-001", "Other", "Person", "other@example.com", dept.Id),
            CancellationToken.None);

        Assert.Equal(EmployeeResultStatus.Conflict, result.Status);
    }

    [Fact]
    public async Task CreateAsync_DepartmentNotFound_ReturnsUnprocessableEntity()
    {
        using var context = CreateContext();
        var service = new EmployeeService(context);

        var result = await service.CreateAsync(
            new CreateEmployeeRequest("EXT-100", "John", "Smith", "john@example.com", Guid.NewGuid()),
            CancellationToken.None);

        Assert.Equal(EmployeeResultStatus.UnprocessableEntity, result.Status);
    }

    // ── UpdateAsync ──────────────────────────────────────────────────────────

    [Fact]
    public async Task UpdateAsync_ValidRequest_ReturnsSuccess()
    {
        using var context = CreateContext();
        var dept = SeedDepartment(context);
        var emp = SeedEmployee(context, dept.Id);
        var service = new EmployeeService(context);

        var result = await service.UpdateAsync(
            emp.Id,
            new UpdateEmployeeRequest("Updated", "Name", "updated@example.com", dept.Id),
            CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal("Updated", result.Value!.FirstName);
        Assert.Equal("Name", result.Value.LastName);
    }

    [Fact]
    public async Task UpdateAsync_ValidRequest_PersistsToDatabase()
    {
        using var context = CreateContext();
        var dept = SeedDepartment(context);
        var emp = SeedEmployee(context, dept.Id);
        var service = new EmployeeService(context);

        await service.UpdateAsync(
            emp.Id,
            new UpdateEmployeeRequest("Updated", "Name", "updated@example.com", dept.Id),
            CancellationToken.None);

        var saved = await context.Employees.FindAsync(emp.Id);
        Assert.Equal("Updated", saved!.FirstName);
        Assert.Equal("Name", saved.LastName);
        Assert.Equal("updated@example.com", saved.Email);
    }

    [Fact]
    public async Task UpdateAsync_UnknownId_ReturnsNotFound()
    {
        using var context = CreateContext();
        var dept = SeedDepartment(context);
        var service = new EmployeeService(context);

        var result = await service.UpdateAsync(
            Guid.NewGuid(),
            new UpdateEmployeeRequest("First", "Last", "email@example.com", dept.Id),
            CancellationToken.None);

        Assert.Equal(EmployeeResultStatus.NotFound, result.Status);
    }

    [Fact]
    public async Task UpdateAsync_DepartmentNotFound_ReturnsUnprocessableEntity()
    {
        using var context = CreateContext();
        var dept = SeedDepartment(context);
        var emp = SeedEmployee(context, dept.Id);
        var service = new EmployeeService(context);

        var result = await service.UpdateAsync(
            emp.Id,
            new UpdateEmployeeRequest("Jane", "Doe", "jane@example.com", Guid.NewGuid()),
            CancellationToken.None);

        Assert.Equal(EmployeeResultStatus.UnprocessableEntity, result.Status);
    }

    // ── DeleteAsync ──────────────────────────────────────────────────────────

    [Fact]
    public async Task DeleteAsync_ExistingEmployee_ReturnsNoContent()
    {
        using var context = CreateContext();
        var dept = SeedDepartment(context);
        var emp = SeedEmployee(context, dept.Id);
        var service = new EmployeeService(context);

        var result = await service.DeleteAsync(emp.Id, CancellationToken.None);

        Assert.Equal(EmployeeResultStatus.NoContent, result.Status);
    }

    [Fact]
    public async Task DeleteAsync_ExistingEmployee_RemovesFromDatabase()
    {
        using var context = CreateContext();
        var dept = SeedDepartment(context);
        var emp = SeedEmployee(context, dept.Id);
        var service = new EmployeeService(context);

        await service.DeleteAsync(emp.Id, CancellationToken.None);

        var deleted = await context.Employees.FindAsync(emp.Id);
        Assert.Null(deleted);
    }

    [Fact]
    public async Task DeleteAsync_UnknownId_ReturnsNotFound()
    {
        using var context = CreateContext();
        var service = new EmployeeService(context);

        var result = await service.DeleteAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.Equal(EmployeeResultStatus.NotFound, result.Status);
    }
}
