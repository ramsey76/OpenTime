using Microsoft.EntityFrameworkCore;
using TimeManagement.Api.DTOs;
using TimeManagement.Domain.Entities;
using TimeManagement.Infrastructure.Database;

namespace TimeManagement.Api.Services.Departments;

public sealed class DepartmentService(AppDbContext context) : IDepartmentService
{
    public async Task<List<DepartmentDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Departments
            .AsNoTracking()
            .Select(d => new DepartmentDto(d.Id, d.Name, d.Code, d.ParentDepartmentId))
            .ToListAsync(cancellationToken);
    }

    public async Task<DepartmentDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Departments
            .AsNoTracking()
            .Where(d => d.Id == id)
            .Select(d => new DepartmentDto(d.Id, d.Name, d.Code, d.ParentDepartmentId))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<DepartmentResult> CreateAsync(CreateDepartmentRequest request, CancellationToken cancellationToken)
    {
        var codeExists = await context.Departments
            .AnyAsync(d => d.Code.ToLower() == request.Code.ToLower(), cancellationToken);

        if (codeExists)
            return DepartmentResult.Conflict($"A department with code '{request.Code}' already exists.");

        if (request.ParentDepartmentId.HasValue)
        {
            var parentExists = await context.Departments
                .AnyAsync(d => d.Id == request.ParentDepartmentId.Value, cancellationToken);

            if (!parentExists)
                return DepartmentResult.UnprocessableEntity($"Parent department '{request.ParentDepartmentId}' does not exist.");
        }

        var department = new Department
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Code = request.Code,
            ParentDepartmentId = request.ParentDepartmentId
        };

        context.Departments.Add(department);
        await context.SaveChangesAsync(cancellationToken);

        return DepartmentResult.Success(new DepartmentDto(department.Id, department.Name, department.Code, department.ParentDepartmentId));
    }

    public async Task<DepartmentResult> UpdateAsync(Guid id, UpdateDepartmentRequest request, CancellationToken cancellationToken)
    {
        var department = await context.Departments.FindAsync([id], cancellationToken);

        if (department is null)
            return DepartmentResult.NotFound();

        var codeConflict = await context.Departments
            .AnyAsync(d => d.Code.ToLower() == request.Code.ToLower() && d.Id != id, cancellationToken);

        if (codeConflict)
            return DepartmentResult.Conflict($"A department with code '{request.Code}' already exists.");

        if (request.ParentDepartmentId.HasValue)
        {
            var parentExists = await context.Departments
                .AnyAsync(d => d.Id == request.ParentDepartmentId.Value, cancellationToken);

            if (!parentExists)
                return DepartmentResult.UnprocessableEntity($"Parent department '{request.ParentDepartmentId}' does not exist.");

            var cycleDetected = await HasCycleAsync(id, request.ParentDepartmentId.Value, cancellationToken);
            if (cycleDetected)
                return DepartmentResult.UnprocessableEntity("Assigning this parent would create a circular reference.");
        }

        department.Name = request.Name;
        department.Code = request.Code;
        department.ParentDepartmentId = request.ParentDepartmentId;

        await context.SaveChangesAsync(cancellationToken);

        return DepartmentResult.Success(new DepartmentDto(department.Id, department.Name, department.Code, department.ParentDepartmentId));
    }

    public async Task<DepartmentResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var department = await context.Departments.FindAsync([id], cancellationToken);

        if (department is null)
            return DepartmentResult.NotFound();

        var hasEmployees = await context.Employees
            .AnyAsync(e => e.DepartmentId == id, cancellationToken);

        if (hasEmployees)
            return DepartmentResult.Conflict("Cannot delete a department that has employees assigned to it.");

        var hasSubDepartments = await context.Departments
            .AnyAsync(d => d.ParentDepartmentId == id, cancellationToken);

        if (hasSubDepartments)
            return DepartmentResult.Conflict("Cannot delete a department that has subdepartments.");

        context.Departments.Remove(department);
        await context.SaveChangesAsync(cancellationToken);

        return DepartmentResult.NoContent();
    }

    private async Task<bool> HasCycleAsync(Guid departmentId, Guid proposedParentId, CancellationToken cancellationToken)
    {
        var visited = new HashSet<Guid>();
        var currentId = (Guid?)proposedParentId;

        while (currentId.HasValue)
        {
            if (currentId.Value == departmentId)
                return true;

            if (!visited.Add(currentId.Value))
                break;

            currentId = await context.Departments
                .Where(d => d.Id == currentId.Value)
                .Select(d => d.ParentDepartmentId)
                .FirstOrDefaultAsync(cancellationToken);
        }

        return false;
    }
}
