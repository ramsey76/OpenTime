using Microsoft.EntityFrameworkCore;
using TimeManagement.Api.DTOs;
using TimeManagement.Domain.Entities;
using TimeManagement.Infrastructure.Database;

namespace TimeManagement.Api.Services.Roles;

public sealed class RoleService(AppDbContext context) : IRoleService
{
    public async Task<List<RoleDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Roles
            .AsNoTracking()
            .Select(r => new RoleDto(r.Id, r.Name, r.Description))
            .ToListAsync(cancellationToken);
    }

    public async Task<RoleDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Roles
            .AsNoTracking()
            .Where(r => r.Id == id)
            .Select(r => new RoleDto(r.Id, r.Name, r.Description))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<RoleResult> CreateAsync(CreateRoleRequest request, CancellationToken cancellationToken)
    {
        var exists = await context.Roles
            .AnyAsync(r => r.Name.ToLower() == request.Name.ToLower(), cancellationToken);

        if (exists)
            return RoleResult.Conflict($"A role with the name '{request.Name}' already exists.");

        var role = new Role
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description ?? string.Empty
        };

        context.Roles.Add(role);
        await context.SaveChangesAsync(cancellationToken);

        return RoleResult.Success(new RoleDto(role.Id, role.Name, role.Description));
    }

    public async Task<RoleResult> UpdateAsync(Guid id, UpdateRoleRequest request, CancellationToken cancellationToken)
    {
        var role = await context.Roles.FindAsync([id], cancellationToken);

        if (role is null)
            return RoleResult.NotFound();

        var nameConflict = await context.Roles
            .AnyAsync(r => r.Name.ToLower() == request.Name.ToLower() && r.Id != id, cancellationToken);

        if (nameConflict)
            return RoleResult.Conflict($"A role with the name '{request.Name}' already exists.");

        role.Name = request.Name;
        role.Description = request.Description ?? string.Empty;

        await context.SaveChangesAsync(cancellationToken);

        return RoleResult.Success(new RoleDto(role.Id, role.Name, role.Description));
    }

    public async Task<RoleResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var role = await context.Roles.FindAsync([id], cancellationToken);

        if (role is null)
            return RoleResult.NotFound();

        var isAssigned = await context.EmployeeRoles
            .AnyAsync(er => er.RoleId == id, cancellationToken);

        if (isAssigned)
            return RoleResult.Conflict("Cannot delete a role that is assigned to one or more employees.");

        context.Roles.Remove(role);
        await context.SaveChangesAsync(cancellationToken);

        return RoleResult.NoContent();
    }
}
