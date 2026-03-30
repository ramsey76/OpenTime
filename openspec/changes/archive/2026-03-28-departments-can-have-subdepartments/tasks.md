## 1. Domain

- [x] 1.1 Add `ParentDepartmentId` (`Guid?`) to `Department.cs`
- [x] 1.2 Add `ParentDepartment` (`Department?`) and `SubDepartments` (`ICollection<Department>`) navigation properties to `Department.cs`

## 2. Infrastructure

- [x] 2.1 Update `DepartmentConfiguration.cs` — configure self-referencing FK (`HasOne(d => d.ParentDepartment).WithMany(d => d.SubDepartments).HasForeignKey(d => d.ParentDepartmentId).OnDelete(DeleteBehavior.Restrict)`)
- [x] 2.2 Run `dotnet ef migrations add AddDepartmentParent` and verify generated migration adds nullable `ParentDepartmentId` column

## 3. DTOs

- [x] 3.1 Add `ParentDepartmentId` (`Guid?`) to `DepartmentDto.cs`
- [x] 3.2 Add optional `ParentDepartmentId` (`Guid?`) to `CreateDepartmentRequest.cs`
- [x] 3.3 Add optional `ParentDepartmentId` (`Guid?`) to `UpdateDepartmentRequest.cs`

## 4. Service Layer

- [x] 4.1 Add `UnprocessableEntity` to `DepartmentResultStatus` enum
- [x] 4.2 Add `DepartmentResult.UnprocessableEntity(string error)` static factory to `DepartmentResult.cs`
- [x] 4.3 Update `GetAllAsync` — include `ParentDepartmentId` in the `DepartmentDto` projection
- [x] 4.4 Update `GetByIdAsync` — include `ParentDepartmentId` in the `DepartmentDto` projection
- [x] 4.5 Update `CreateAsync` — if `ParentDepartmentId` provided: (a) verify parent exists, return `UnprocessableEntity` if not; (b) detect cycle (ancestor walk), return `UnprocessableEntity` if cycle detected; persist with `ParentDepartmentId`
- [x] 4.6 Update `UpdateAsync` — same parent-exists and cycle checks as create; also support setting `ParentDepartmentId` to null (remove parent)
- [x] 4.7 Update `DeleteAsync` — add check: if any department has `ParentDepartmentId == id`, return `Conflict`

## 5. Handlers

- [x] 5.1 Update `CreateDepartment.cs` handler — map `DepartmentResultStatus.UnprocessableEntity` to `Results.UnprocessableEntity(result.Error)`
- [x] 5.2 Update `UpdateDepartment.cs` handler — map `DepartmentResultStatus.UnprocessableEntity` to `Results.UnprocessableEntity(result.Error)`

## 6. Endpoint Registration

- [x] 6.1 Update `DepartmentEndpoints.cs` — add `.Produces(StatusCodes.Status422UnprocessableEntity)` to `CreateDepartment` and `UpdateDepartment` route registrations

## 7. Unit Tests

- [x] 7.1 Test: `CreateAsync_WithValidParent_ReturnsSuccessWithParentId` — seed a parent, create child with parentId, assert dto.ParentDepartmentId set correctly
- [x] 7.2 Test: `CreateAsync_ParentNotFound_ReturnsUnprocessableEntity` — create with non-existent parentId, assert `Status = UnprocessableEntity`
- [x] 7.3 Test: `CreateAsync_WithoutParent_ReturnsSuccessWithNullParentId` — create without parentId, assert dto.ParentDepartmentId is null
- [x] 7.4 Test: `UpdateAsync_AssignsParent_ReturnsSuccess` — seed dept and parent, update dept to set parent
- [x] 7.5 Test: `UpdateAsync_RemovesParent_ReturnsSuccess` — seed dept with parent, update to set parentId null
- [x] 7.6 Test: `UpdateAsync_ParentNotFound_ReturnsUnprocessableEntity` — update with non-existent parentId, assert `Status = UnprocessableEntity`
- [x] 7.7 Test: `UpdateAsync_CircularReference_DirectSelf_ReturnsUnprocessableEntity` — try to set a department as its own parent
- [x] 7.8 Test: `UpdateAsync_CircularReference_IndirectCycle_ReturnsUnprocessableEntity` — seed A→B→C chain, try to set A's parent to C
- [x] 7.9 Test: `DeleteAsync_DepartmentWithSubdepartments_ReturnsConflict` — seed parent with child, attempt delete of parent, assert `Status = Conflict`

## 8. Verify

- [x] 8.1 Run `dotnet build` and confirm 0 errors
- [x] 8.2 Run `dotnet test` and confirm all tests pass (existing + new)
- [x] 8.3 Run `dotnet ef database update` to apply migration to local DB
- [ ] 8.4 Confirm Swagger shows `parentDepartmentId` on create/update/get endpoints and `422` responses
