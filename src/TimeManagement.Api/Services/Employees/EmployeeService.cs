using Microsoft.EntityFrameworkCore;
using TimeManagement.Api.DTOs;
using TimeManagement.Domain.Entities;
using TimeManagement.Infrastructure.Database;

namespace TimeManagement.Api.Services.Employees;

public sealed class EmployeeService(AppDbContext context) : IEmployeeService
{
    public async Task<List<EmployeeDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Employees
            .AsNoTracking()
            .Select(e => new EmployeeDto(e.Id, e.ExternalId, e.FirstName, e.LastName, e.Email, e.DepartmentId))
            .ToListAsync(cancellationToken);
    }

    public async Task<EmployeeDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Employees
            .AsNoTracking()
            .Where(e => e.Id == id)
            .Select(e => new EmployeeDto(e.Id, e.ExternalId, e.FirstName, e.LastName, e.Email, e.DepartmentId))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<EmployeeResult> CreateAsync(CreateEmployeeRequest request, CancellationToken cancellationToken)
    {
        var externalIdExists = await context.Employees
            .AnyAsync(e => e.ExternalId == request.ExternalId, cancellationToken);

        if (externalIdExists)
            return EmployeeResult.Conflict($"An employee with ExternalId '{request.ExternalId}' already exists.");

        var departmentExists = await context.Departments
            .AnyAsync(d => d.Id == request.DepartmentId, cancellationToken);

        if (!departmentExists)
            return EmployeeResult.UnprocessableEntity($"Department '{request.DepartmentId}' does not exist.");

        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            ExternalId = request.ExternalId,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            DepartmentId = request.DepartmentId
        };

        context.Employees.Add(employee);
        await context.SaveChangesAsync(cancellationToken);

        return EmployeeResult.Success(new EmployeeDto(employee.Id, employee.ExternalId, employee.FirstName, employee.LastName, employee.Email, employee.DepartmentId));
    }

    public async Task<EmployeeResult> UpdateAsync(Guid id, UpdateEmployeeRequest request, CancellationToken cancellationToken)
    {
        var employee = await context.Employees.FindAsync([id], cancellationToken);

        if (employee is null)
            return EmployeeResult.NotFound();

        var departmentExists = await context.Departments
            .AnyAsync(d => d.Id == request.DepartmentId, cancellationToken);

        if (!departmentExists)
            return EmployeeResult.UnprocessableEntity($"Department '{request.DepartmentId}' does not exist.");

        employee.FirstName = request.FirstName;
        employee.LastName = request.LastName;
        employee.Email = request.Email;
        employee.DepartmentId = request.DepartmentId;

        await context.SaveChangesAsync(cancellationToken);

        return EmployeeResult.Success(new EmployeeDto(employee.Id, employee.ExternalId, employee.FirstName, employee.LastName, employee.Email, employee.DepartmentId));
    }

    public async Task<EmployeeResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var employee = await context.Employees.FindAsync([id], cancellationToken);

        if (employee is null)
            return EmployeeResult.NotFound();

        context.Employees.Remove(employee);
        await context.SaveChangesAsync(cancellationToken);

        return EmployeeResult.NoContent();
    }
}
