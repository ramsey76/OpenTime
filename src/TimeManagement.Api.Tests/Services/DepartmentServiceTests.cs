using Microsoft.EntityFrameworkCore;
using TimeManagement.Api.DTOs;
using TimeManagement.Api.Services.Departments;
using TimeManagement.Domain.Entities;
using TimeManagement.Infrastructure.Database;

namespace TimeManagement.Api.Tests.Services;

public class DepartmentServiceTests
{
    private static AppDbContext CreateContext() =>
        new(new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    // ── GetAllAsync ──────────────────────────────────────────────────────────

    [Fact]
    public async Task GetAllAsync_WithDepartments_ReturnsAllDepartmentDtos()
    {
        using var context = CreateContext();
        context.Departments.AddRange(
            new Department { Id = Guid.NewGuid(), Name = "Engineering", Code = "ENG" },
            new Department { Id = Guid.NewGuid(), Name = "Finance", Code = "FIN" });
        await context.SaveChangesAsync();

        var service = new DepartmentService(context);
        var result = await service.GetAllAsync(CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.Contains(result, d => d.Name == "Engineering" && d.Code == "ENG");
        Assert.Contains(result, d => d.Name == "Finance" && d.Code == "FIN");
    }

    [Fact]
    public async Task GetAllAsync_Empty_ReturnsEmptyList()
    {
        using var context = CreateContext();
        var service = new DepartmentService(context);

        var result = await service.GetAllAsync(CancellationToken.None);

        Assert.Empty(result);
    }

    // ── GetByIdAsync ─────────────────────────────────────────────────────────

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsDepartmentDto()
    {
        using var context = CreateContext();
        var id = Guid.NewGuid();
        context.Departments.Add(new Department { Id = id, Name = "HR", Code = "HR" });
        await context.SaveChangesAsync();

        var service = new DepartmentService(context);
        var result = await service.GetByIdAsync(id, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal("HR", result.Name);
        Assert.Equal("HR", result.Code);
    }

    [Fact]
    public async Task GetByIdAsync_UnknownId_ReturnsNull()
    {
        using var context = CreateContext();
        var service = new DepartmentService(context);

        var result = await service.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.Null(result);
    }

    // ── CreateAsync ──────────────────────────────────────────────────────────

    [Fact]
    public async Task CreateAsync_ValidRequest_ReturnsSuccess()
    {
        using var context = CreateContext();
        var service = new DepartmentService(context);

        var result = await service.CreateAsync(
            new CreateDepartmentRequest("Marketing", "MKT"), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("Marketing", result.Value.Name);
        Assert.Equal("MKT", result.Value.Code);
        Assert.NotEqual(Guid.Empty, result.Value.Id);
    }

    [Fact]
    public async Task CreateAsync_ValidRequest_PersistsToDatabase()
    {
        using var context = CreateContext();
        var service = new DepartmentService(context);

        await service.CreateAsync(new CreateDepartmentRequest("Legal", "LEG"), CancellationToken.None);

        var saved = await context.Departments.FirstOrDefaultAsync(d => d.Code == "LEG");
        Assert.NotNull(saved);
        Assert.Equal("Legal", saved.Name);
    }

    [Fact]
    public async Task CreateAsync_DuplicateCode_ReturnsConflict()
    {
        using var context = CreateContext();
        context.Departments.Add(new Department { Id = Guid.NewGuid(), Name = "Engineering", Code = "ENG" });
        await context.SaveChangesAsync();

        var service = new DepartmentService(context);
        var result = await service.CreateAsync(
            new CreateDepartmentRequest("Extended Engineering", "eng"), CancellationToken.None);

        Assert.Equal(DepartmentResultStatus.Conflict, result.Status);
    }

    // ── UpdateAsync ──────────────────────────────────────────────────────────

    [Fact]
    public async Task UpdateAsync_ValidRequest_ReturnsSuccess()
    {
        using var context = CreateContext();
        var id = Guid.NewGuid();
        context.Departments.Add(new Department { Id = id, Name = "OldName", Code = "OLD" });
        await context.SaveChangesAsync();

        var service = new DepartmentService(context);
        var result = await service.UpdateAsync(
            id, new UpdateDepartmentRequest("NewName", "NEW"), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("NewName", result.Value.Name);
        Assert.Equal("NEW", result.Value.Code);
    }

    [Fact]
    public async Task UpdateAsync_ValidRequest_PersistsToDatabase()
    {
        using var context = CreateContext();
        var id = Guid.NewGuid();
        context.Departments.Add(new Department { Id = id, Name = "OldName", Code = "OLD" });
        await context.SaveChangesAsync();

        var service = new DepartmentService(context);
        await service.UpdateAsync(id, new UpdateDepartmentRequest("NewName", "NEW"), CancellationToken.None);

        var saved = await context.Departments.FindAsync(id);
        Assert.Equal("NewName", saved!.Name);
        Assert.Equal("NEW", saved.Code);
    }

    [Fact]
    public async Task UpdateAsync_UnknownId_ReturnsNotFound()
    {
        using var context = CreateContext();
        var service = new DepartmentService(context);

        var result = await service.UpdateAsync(
            Guid.NewGuid(), new UpdateDepartmentRequest("Any", "ANY"), CancellationToken.None);

        Assert.Equal(DepartmentResultStatus.NotFound, result.Status);
    }

    [Fact]
    public async Task UpdateAsync_CodeTakenByOtherDepartment_ReturnsConflict()
    {
        using var context = CreateContext();
        var id1 = Guid.NewGuid();
        context.Departments.AddRange(
            new Department { Id = id1, Name = "DeptA", Code = "AAA" },
            new Department { Id = Guid.NewGuid(), Name = "DeptB", Code = "BBB" });
        await context.SaveChangesAsync();

        var service = new DepartmentService(context);
        var result = await service.UpdateAsync(
            id1, new UpdateDepartmentRequest("DeptA", "BBB"), CancellationToken.None);

        Assert.Equal(DepartmentResultStatus.Conflict, result.Status);
    }

    // ── DeleteAsync ──────────────────────────────────────────────────────────

    [Fact]
    public async Task DeleteAsync_UnassignedDepartment_ReturnsNoContent()
    {
        using var context = CreateContext();
        var id = Guid.NewGuid();
        context.Departments.Add(new Department { Id = id, Name = "Temp", Code = "TMP" });
        await context.SaveChangesAsync();

        var service = new DepartmentService(context);
        var result = await service.DeleteAsync(id, CancellationToken.None);

        Assert.Equal(DepartmentResultStatus.NoContent, result.Status);
    }

    [Fact]
    public async Task DeleteAsync_UnassignedDepartment_RemovesFromDatabase()
    {
        using var context = CreateContext();
        var id = Guid.NewGuid();
        context.Departments.Add(new Department { Id = id, Name = "Temp", Code = "TMP" });
        await context.SaveChangesAsync();

        var service = new DepartmentService(context);
        await service.DeleteAsync(id, CancellationToken.None);

        var deleted = await context.Departments.FindAsync(id);
        Assert.Null(deleted);
    }

    [Fact]
    public async Task DeleteAsync_UnknownId_ReturnsNotFound()
    {
        using var context = CreateContext();
        var service = new DepartmentService(context);

        var result = await service.DeleteAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.Equal(DepartmentResultStatus.NotFound, result.Status);
    }

    [Fact]
    public async Task DeleteAsync_DepartmentWithEmployees_ReturnsConflict()
    {
        using var context = CreateContext();
        var deptId = Guid.NewGuid();
        context.Departments.Add(new Department { Id = deptId, Name = "Engineering", Code = "ENG" });
        context.Employees.Add(new Employee
        {
            Id = Guid.NewGuid(),
            ExternalId = "EMP001",
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane.doe@example.com",
            DepartmentId = deptId
        });
        await context.SaveChangesAsync();

        var service = new DepartmentService(context);
        var result = await service.DeleteAsync(deptId, CancellationToken.None);

        Assert.Equal(DepartmentResultStatus.Conflict, result.Status);
    }

    // ── Subdepartment tests ───────────────────────────────────────────────────

    [Fact]
    public async Task CreateAsync_WithValidParent_ReturnsSuccessWithParentId()
    {
        using var context = CreateContext();
        var parentId = Guid.NewGuid();
        context.Departments.Add(new Department { Id = parentId, Name = "Parent", Code = "PAR" });
        await context.SaveChangesAsync();

        var service = new DepartmentService(context);
        var result = await service.CreateAsync(
            new CreateDepartmentRequest("Child", "CHD", parentId), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(parentId, result.Value!.ParentDepartmentId);
    }

    [Fact]
    public async Task CreateAsync_ParentNotFound_ReturnsUnprocessableEntity()
    {
        using var context = CreateContext();
        var service = new DepartmentService(context);

        var result = await service.CreateAsync(
            new CreateDepartmentRequest("Child", "CHD", Guid.NewGuid()), CancellationToken.None);

        Assert.Equal(DepartmentResultStatus.UnprocessableEntity, result.Status);
    }

    [Fact]
    public async Task CreateAsync_WithoutParent_ReturnsSuccessWithNullParentId()
    {
        using var context = CreateContext();
        var service = new DepartmentService(context);

        var result = await service.CreateAsync(
            new CreateDepartmentRequest("Root", "ROOT"), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Null(result.Value!.ParentDepartmentId);
    }

    [Fact]
    public async Task UpdateAsync_AssignsParent_ReturnsSuccess()
    {
        using var context = CreateContext();
        var deptId = Guid.NewGuid();
        var parentId = Guid.NewGuid();
        context.Departments.AddRange(
            new Department { Id = deptId, Name = "Child", Code = "CHD" },
            new Department { Id = parentId, Name = "Parent", Code = "PAR" });
        await context.SaveChangesAsync();

        var service = new DepartmentService(context);
        var result = await service.UpdateAsync(
            deptId, new UpdateDepartmentRequest("Child", "CHD", parentId), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(parentId, result.Value!.ParentDepartmentId);
    }

    [Fact]
    public async Task UpdateAsync_RemovesParent_ReturnsSuccess()
    {
        using var context = CreateContext();
        var parentId = Guid.NewGuid();
        var deptId = Guid.NewGuid();
        context.Departments.AddRange(
            new Department { Id = parentId, Name = "Parent", Code = "PAR" },
            new Department { Id = deptId, Name = "Child", Code = "CHD", ParentDepartmentId = parentId });
        await context.SaveChangesAsync();

        var service = new DepartmentService(context);
        var result = await service.UpdateAsync(
            deptId, new UpdateDepartmentRequest("Child", "CHD", null), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Null(result.Value!.ParentDepartmentId);
    }

    [Fact]
    public async Task UpdateAsync_ParentNotFound_ReturnsUnprocessableEntity()
    {
        using var context = CreateContext();
        var deptId = Guid.NewGuid();
        context.Departments.Add(new Department { Id = deptId, Name = "Dept", Code = "DPT" });
        await context.SaveChangesAsync();

        var service = new DepartmentService(context);
        var result = await service.UpdateAsync(
            deptId, new UpdateDepartmentRequest("Dept", "DPT", Guid.NewGuid()), CancellationToken.None);

        Assert.Equal(DepartmentResultStatus.UnprocessableEntity, result.Status);
    }

    [Fact]
    public async Task UpdateAsync_CircularReference_DirectSelf_ReturnsUnprocessableEntity()
    {
        using var context = CreateContext();
        var deptId = Guid.NewGuid();
        context.Departments.Add(new Department { Id = deptId, Name = "Dept", Code = "DPT" });
        await context.SaveChangesAsync();

        var service = new DepartmentService(context);
        var result = await service.UpdateAsync(
            deptId, new UpdateDepartmentRequest("Dept", "DPT", deptId), CancellationToken.None);

        Assert.Equal(DepartmentResultStatus.UnprocessableEntity, result.Status);
    }

    [Fact]
    public async Task UpdateAsync_CircularReference_IndirectCycle_ReturnsUnprocessableEntity()
    {
        using var context = CreateContext();
        var idA = Guid.NewGuid();
        var idB = Guid.NewGuid();
        var idC = Guid.NewGuid();
        // A → B → C
        context.Departments.AddRange(
            new Department { Id = idA, Name = "A", Code = "AAA" },
            new Department { Id = idB, Name = "B", Code = "BBB", ParentDepartmentId = idA },
            new Department { Id = idC, Name = "C", Code = "CCC", ParentDepartmentId = idB });
        await context.SaveChangesAsync();

        var service = new DepartmentService(context);
        // Try to set A's parent to C — would create A→B→C→A cycle
        var result = await service.UpdateAsync(
            idA, new UpdateDepartmentRequest("A", "AAA", idC), CancellationToken.None);

        Assert.Equal(DepartmentResultStatus.UnprocessableEntity, result.Status);
    }

    [Fact]
    public async Task DeleteAsync_DepartmentWithSubdepartments_ReturnsConflict()
    {
        using var context = CreateContext();
        var parentId = Guid.NewGuid();
        var childId = Guid.NewGuid();
        context.Departments.AddRange(
            new Department { Id = parentId, Name = "Parent", Code = "PAR" },
            new Department { Id = childId, Name = "Child", Code = "CHD", ParentDepartmentId = parentId });
        await context.SaveChangesAsync();

        var service = new DepartmentService(context);
        var result = await service.DeleteAsync(parentId, CancellationToken.None);

        Assert.Equal(DepartmentResultStatus.Conflict, result.Status);
    }
}
