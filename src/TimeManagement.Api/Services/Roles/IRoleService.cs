using TimeManagement.Api.DTOs;

namespace TimeManagement.Api.Services.Roles;

public interface IRoleService
{
    Task<List<RoleDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<RoleDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<RoleResult> CreateAsync(CreateRoleRequest request, CancellationToken cancellationToken);
    Task<RoleResult> UpdateAsync(Guid id, UpdateRoleRequest request, CancellationToken cancellationToken);
    Task<RoleResult> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
