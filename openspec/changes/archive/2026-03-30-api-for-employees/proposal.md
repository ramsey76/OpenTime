## Why

The employee data model exists but there are no HTTP endpoints to manage employee records. Administrators need a REST API to create, retrieve, update, and delete employees, consistent with the existing department API pattern.

## What Changes

- Add `EmployeeEndpoints` endpoint group exposing CRUD routes under `/api/employees`
- Add one thin handler class per operation (`GetEmployees`, `GetEmployeeById`, `CreateEmployee`, `UpdateEmployee`, `DeleteEmployee`)
- Add `IEmployeeService` / `EmployeeService` containing all business logic and EF Core data access
- Add request/response DTOs (`EmployeeDto`, `CreateEmployeeRequest`, `UpdateEmployeeRequest`)

## Capabilities

### New Capabilities

- `employee-management-api`: HTTP REST API for listing, retrieving, creating, updating, and deleting employee records, including validation rules and error responses

### Modified Capabilities

_(none)_

## Impact

- **Backend**: New endpoint group, handler classes, service interface/implementation, and DTOs in the `TimeManagement` namespace following the api-minimal pattern
- **Database**: No schema changes — uses existing `Employees` table defined in `employee-management` spec
- **API surface**: New endpoints at `/api/employees`
- **Dependencies**: Requires `Departments` to exist (foreign key on `DepartmentId`)
