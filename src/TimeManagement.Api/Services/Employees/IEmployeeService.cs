using TimeManagement.Api.DTOs;

namespace TimeManagement.Api.Services.Employees;

public interface IEmployeeService
{
    Task<List<EmployeeDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<EmployeeDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<EmployeeResult> CreateAsync(CreateEmployeeRequest request, CancellationToken cancellationToken);
    Task<EmployeeResult> UpdateAsync(Guid id, UpdateEmployeeRequest request, CancellationToken cancellationToken);
    Task<EmployeeResult> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
