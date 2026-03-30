## 1. DTOs

- [x] 1.1 Create `DTOs/DepartmentDto.cs` with `Id`, `Name`, `Code`
- [x] 1.2 Create `DTOs/CreateDepartmentRequest.cs` with `Name` (required, max 100), `Code` (required, max 10)
- [x] 1.3 Create `DTOs/UpdateDepartmentRequest.cs` with `Name` (required, max 100), `Code` (required, max 10)

## 2. Service Layer

- [x] 2.1 Create `Services/Departments/DepartmentResult.cs` with `IsSuccess`, `Value`, `Error`, `DepartmentResultStatus` (Ok / NoContent / NotFound / Conflict)
- [x] 2.2 Create `Services/Departments/IDepartmentService.cs` interface with `GetAllAsync`, `GetByIdAsync`, `CreateAsync`, `UpdateAsync`, `DeleteAsync`
- [x] 2.3 Create `Services/Departments/DepartmentService.cs` implementing `IDepartmentService` with `AppDbContext` injected
- [x] 2.4 Implement `GetAllAsync` — return all departments as `List<DepartmentDto>` using `AsNoTracking`
- [x] 2.5 Implement `GetByIdAsync` — return `DepartmentDto` or `null` if not found
- [x] 2.6 Implement `CreateAsync` — check for duplicate `Code` (case-insensitive), persist, return result
- [x] 2.7 Implement `UpdateAsync` — find by ID, check duplicate `Code` on other departments, update, return result
- [x] 2.8 Implement `DeleteAsync` — find by ID, check for existing `Employees` assigned to department, delete, return result
- [x] 2.9 Register `IDepartmentService` / `DepartmentService` as scoped in `Program.cs`

## 3. Endpoints

- [x] 3.1 Create `Endpoints/Departments/GetDepartments.cs` handler — calls `GetAllAsync`, returns `200 OK`
- [x] 3.2 Create `Endpoints/Departments/GetDepartmentById.cs` handler — calls `GetByIdAsync`, returns `200 OK` or `404 Not Found`
- [x] 3.3 Create `Endpoints/Departments/CreateDepartment.cs` handler — calls `CreateAsync`, returns `201 Created` with `Location` header, `400`, or `409`
- [x] 3.4 Create `Endpoints/Departments/UpdateDepartment.cs` handler — calls `UpdateAsync`, returns `200 OK`, `400`, `404`, or `409`
- [x] 3.5 Create `Endpoints/Departments/DeleteDepartment.cs` handler — calls `DeleteAsync`, returns `204 No Content`, `404`, or `409`
- [x] 3.6 Create `Endpoints/Departments/DepartmentEndpoints.cs` — registers all 5 routes under `/api/departments` with tags and typed `Produces` declarations

## 4. Unit Tests

- [x] 4.1 Test: `GetAllAsync_WithDepartments_ReturnsAllDepartmentDtos` — seed two departments, assert both returned with correct fields
- [x] 4.2 Test: `GetAllAsync_Empty_ReturnsEmptyList`
- [x] 4.3 Test: `GetByIdAsync_ExistingId_ReturnsDepartmentDto`
- [x] 4.4 Test: `GetByIdAsync_UnknownId_ReturnsNull`
- [x] 4.5 Test: `CreateAsync_ValidRequest_ReturnsSuccess` — assert `IsSuccess = true`, correct name/code and non-empty Id
- [x] 4.6 Test: `CreateAsync_ValidRequest_PersistsToDatabase`
- [x] 4.7 Test: `CreateAsync_DuplicateCode_ReturnsConflict` — seed a department, create with same code (different case), assert `Status = Conflict`
- [x] 4.8 Test: `UpdateAsync_ValidRequest_ReturnsSuccess`
- [x] 4.9 Test: `UpdateAsync_ValidRequest_PersistsToDatabase`
- [x] 4.10 Test: `UpdateAsync_UnknownId_ReturnsNotFound`
- [x] 4.11 Test: `UpdateAsync_CodeTakenByOtherDepartment_ReturnsConflict`
- [x] 4.12 Test: `DeleteAsync_UnassignedDepartment_ReturnsNoContent`
- [x] 4.13 Test: `DeleteAsync_UnassignedDepartment_RemovesFromDatabase`
- [x] 4.14 Test: `DeleteAsync_UnknownId_ReturnsNotFound`
- [x] 4.15 Test: `DeleteAsync_DepartmentWithEmployees_ReturnsConflict` — seed department + Employee with DepartmentId set, assert `Status = Conflict`

## 5. Verify

- [x] 5.1 Run `dotnet build` and confirm 0 errors
- [x] 5.2 Run `dotnet test` and confirm all tests pass
- [x] 5.3 Run the application and confirm all 5 department endpoints appear in Swagger UI
