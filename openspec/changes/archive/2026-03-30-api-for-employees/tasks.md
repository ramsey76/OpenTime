## 1. DTOs

- [x] 1.1 Create `EmployeeDto` sealed record (`Id`, `ExternalId`, `FirstName`, `LastName`, `Email`, `DepartmentId`)
- [x] 1.2 Create `CreateEmployeeRequest` with data annotations (`[Required]`, `[MaxLength]`) for all required fields
- [x] 1.3 Create `UpdateEmployeeRequest` (same fields as Create, excluding `ExternalId`)

## 2. Service Layer

- [x] 2.1 Create `EmployeeResult` sealed record with `IsSuccess`, `Value`, `Error`, and `Status` enum (`Ok`, `NoContent`, `NotFound`, `Conflict`, `UnprocessableEntity`) — mirror `DepartmentResult`
- [x] 2.2 Create `IEmployeeService` interface with `GetAllAsync`, `GetByIdAsync`, `CreateAsync`, `UpdateAsync`, `DeleteAsync`
- [x] 2.3 Implement `EmployeeService.GetAllAsync` — query `Employees` with `AsNoTracking`, project to `EmployeeDto`
- [x] 2.4 Implement `EmployeeService.GetByIdAsync` — return `EmployeeDto` or `null`
- [x] 2.5 Implement `EmployeeService.CreateAsync` — check `ExternalId` uniqueness, validate `DepartmentId` exists, persist and return `201` result
- [x] 2.6 Implement `EmployeeService.UpdateAsync` — find employee (404 if missing), validate `DepartmentId` exists, update fields and save
- [x] 2.7 Implement `EmployeeService.DeleteAsync` — find employee (404 if missing), delete and return `NoContent`

## 3. Endpoint Handlers

- [x] 3.1 Create `GetEmployees.cs` — call `GetAllAsync`, return `200 OK` with list
- [x] 3.2 Create `GetEmployeeById.cs` — call `GetByIdAsync`, return `200 OK` or `404 Not Found`
- [x] 3.3 Create `CreateEmployee.cs` — call `CreateAsync`, map result to `201 Created` (with `Location` header) or error response
- [x] 3.4 Create `UpdateEmployee.cs` — call `UpdateAsync`, map result to `200 OK`, `404`, `422`
- [x] 3.5 Create `DeleteEmployee.cs` — call `DeleteAsync`, map result to `204 No Content` or `404`
- [x] 3.6 Create `EmployeeEndpoints.cs` inheriting `EndPoints` — register all five routes on `/api/employees` group with `WithTags("Employees")`

## 4. DI Registration

- [x] 4.1 Register `builder.Services.AddScoped<IEmployeeService, EmployeeService>()` in `Program.cs`

## 5. Tests

- [x] 5.1 Test `GetAllAsync` — returns mapped DTOs; returns empty list when no employees
- [x] 5.2 Test `GetByIdAsync` — returns DTO for existing id; returns null for unknown id
- [x] 5.3 Test `CreateAsync` — success path; duplicate `ExternalId` returns `Conflict`; invalid `DepartmentId` returns `UnprocessableEntity`; missing required fields return `BadRequest`
- [x] 5.4 Test `UpdateAsync` — success path; not found returns `NotFound`; invalid `DepartmentId` returns `UnprocessableEntity`
- [x] 5.5 Test `DeleteAsync` — success returns `NoContent`; not found returns `NotFound`
