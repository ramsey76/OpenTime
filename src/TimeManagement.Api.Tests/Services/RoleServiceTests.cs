using Microsoft.EntityFrameworkCore;
using TimeManagement.Api.DTOs;
using TimeManagement.Api.Services.Roles;
using TimeManagement.Domain.Entities;
using TimeManagement.Infrastructure.Database;

namespace TimeManagement.Api.Tests.Services;

public class RoleServiceTests
{
    private static AppDbContext CreateContext() =>
        new(new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    // ── GetAllAsync ──────────────────────────────────────────────────────────

    [Fact]
    public async Task GetAllAsync_WithRoles_ReturnsAllRoleDtos()
    {
        using var context = CreateContext();
        context.Roles.AddRange(
            new Role { Id = Guid.NewGuid(), Name = "Developer", Description = "Dev role" },
            new Role { Id = Guid.NewGuid(), Name = "Manager", Description = "Mgr role" });
        await context.SaveChangesAsync();

        var service = new RoleService(context);
        var result = await service.GetAllAsync(CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.Contains(result, r => r.Name == "Developer" && r.Description == "Dev role");
        Assert.Contains(result, r => r.Name == "Manager" && r.Description == "Mgr role");
    }

    [Fact]
    public async Task GetAllAsync_Empty_ReturnsEmptyList()
    {
        using var context = CreateContext();
        var service = new RoleService(context);

        var result = await service.GetAllAsync(CancellationToken.None);

        Assert.Empty(result);
    }

    // ── GetByIdAsync ─────────────────────────────────────────────────────────

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsRoleDto()
    {
        using var context = CreateContext();
        var id = Guid.NewGuid();
        context.Roles.Add(new Role { Id = id, Name = "Tester", Description = "QA" });
        await context.SaveChangesAsync();

        var service = new RoleService(context);
        var result = await service.GetByIdAsync(id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal("Tester", result.Name);
        Assert.Equal("QA", result.Description);
    }

    [Fact]
    public async Task GetByIdAsync_UnknownId_ReturnsNull()
    {
        using var context = CreateContext();
        var service = new RoleService(context);

        var result = await service.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.Null(result);
    }

    // ── CreateAsync ──────────────────────────────────────────────────────────

    [Fact]
    public async Task CreateAsync_ValidRequest_ReturnsSuccess()
    {
        using var context = CreateContext();
        var service = new RoleService(context);

        var result = await service.CreateAsync(
            new CreateRoleRequest("Designer", null), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("Designer", result.Value.Name);
        Assert.NotEqual(Guid.Empty, result.Value.Id);
    }

    [Fact]
    public async Task CreateAsync_ValidRequest_PersistsToDatabase()
    {
        using var context = CreateContext();
        var service = new RoleService(context);

        await service.CreateAsync(new CreateRoleRequest("Analyst", "Data analyst"), CancellationToken.None);

        var saved = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Analyst");
        Assert.NotNull(saved);
        Assert.Equal("Data analyst", saved.Description);
    }

    [Fact]
    public async Task CreateAsync_DuplicateName_ReturnsConflict()
    {
        using var context = CreateContext();
        context.Roles.Add(new Role { Id = Guid.NewGuid(), Name = "Developer", Description = "" });
        await context.SaveChangesAsync();

        var service = new RoleService(context);
        var result = await service.CreateAsync(
            new CreateRoleRequest("DEVELOPER", null), CancellationToken.None);

        Assert.Equal(RoleResultStatus.Conflict, result.Status);
    }

    // ── UpdateAsync ──────────────────────────────────────────────────────────

    [Fact]
    public async Task UpdateAsync_ValidRequest_ReturnsSuccess()
    {
        using var context = CreateContext();
        var id = Guid.NewGuid();
        context.Roles.Add(new Role { Id = id, Name = "OldName", Description = "" });
        await context.SaveChangesAsync();

        var service = new RoleService(context);
        var result = await service.UpdateAsync(
            id, new UpdateRoleRequest("NewName", "Updated"), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("NewName", result.Value.Name);
        Assert.Equal("Updated", result.Value.Description);
    }

    [Fact]
    public async Task UpdateAsync_ValidRequest_PersistsToDatabase()
    {
        using var context = CreateContext();
        var id = Guid.NewGuid();
        context.Roles.Add(new Role { Id = id, Name = "OldName", Description = "" });
        await context.SaveChangesAsync();

        var service = new RoleService(context);
        await service.UpdateAsync(id, new UpdateRoleRequest("NewName", "Desc"), CancellationToken.None);

        var saved = await context.Roles.FindAsync(id);
        Assert.Equal("NewName", saved!.Name);
        Assert.Equal("Desc", saved.Description);
    }

    [Fact]
    public async Task UpdateAsync_UnknownId_ReturnsNotFound()
    {
        using var context = CreateContext();
        var service = new RoleService(context);

        var result = await service.UpdateAsync(
            Guid.NewGuid(), new UpdateRoleRequest("Any", null), CancellationToken.None);

        Assert.Equal(RoleResultStatus.NotFound, result.Status);
    }

    [Fact]
    public async Task UpdateAsync_NameTakenByOtherRole_ReturnsConflict()
    {
        using var context = CreateContext();
        var id1 = Guid.NewGuid();
        context.Roles.AddRange(
            new Role { Id = id1, Name = "RoleA", Description = "" },
            new Role { Id = Guid.NewGuid(), Name = "RoleB", Description = "" });
        await context.SaveChangesAsync();

        var service = new RoleService(context);
        var result = await service.UpdateAsync(
            id1, new UpdateRoleRequest("RoleB", null), CancellationToken.None);

        Assert.Equal(RoleResultStatus.Conflict, result.Status);
    }

    // ── DeleteAsync ──────────────────────────────────────────────────────────

    [Fact]
    public async Task DeleteAsync_UnassignedRole_ReturnsNoContent()
    {
        using var context = CreateContext();
        var id = Guid.NewGuid();
        context.Roles.Add(new Role { Id = id, Name = "Temp", Description = "" });
        await context.SaveChangesAsync();

        var service = new RoleService(context);
        var result = await service.DeleteAsync(id, CancellationToken.None);

        Assert.Equal(RoleResultStatus.NoContent, result.Status);
    }

    [Fact]
    public async Task DeleteAsync_UnassignedRole_RemovesFromDatabase()
    {
        using var context = CreateContext();
        var id = Guid.NewGuid();
        context.Roles.Add(new Role { Id = id, Name = "Temp", Description = "" });
        await context.SaveChangesAsync();

        var service = new RoleService(context);
        await service.DeleteAsync(id, CancellationToken.None);

        var deleted = await context.Roles.FindAsync(id);
        Assert.Null(deleted);
    }

    [Fact]
    public async Task DeleteAsync_UnknownId_ReturnsNotFound()
    {
        using var context = CreateContext();
        var service = new RoleService(context);

        var result = await service.DeleteAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.Equal(RoleResultStatus.NotFound, result.Status);
    }

    [Fact]
    public async Task DeleteAsync_AssignedRole_ReturnsConflict()
    {
        using var context = CreateContext();
        var roleId = Guid.NewGuid();
        var employeeId = Guid.NewGuid();
        context.Roles.Add(new Role { Id = roleId, Name = "Assigned", Description = "" });
        context.EmployeeRoles.Add(new EmployeeRole { RoleId = roleId, EmployeeId = employeeId });
        await context.SaveChangesAsync();

        var service = new RoleService(context);
        var result = await service.DeleteAsync(roleId, CancellationToken.None);

        Assert.Equal(RoleResultStatus.Conflict, result.Status);
    }
}
