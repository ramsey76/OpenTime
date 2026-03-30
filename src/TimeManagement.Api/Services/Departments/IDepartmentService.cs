using TimeManagement.Api.DTOs;

namespace TimeManagement.Api.Services.Departments;

public interface IDepartmentService
{
    Task<List<DepartmentDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<DepartmentDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<DepartmentResult> CreateAsync(CreateDepartmentRequest request, CancellationToken cancellationToken);
    Task<DepartmentResult> UpdateAsync(Guid id, UpdateDepartmentRequest request, CancellationToken cancellationToken);
    Task<DepartmentResult> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
