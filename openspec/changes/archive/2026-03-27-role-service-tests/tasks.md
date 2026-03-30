## 1. Project Setup

- [x] 1.1 Create `src/TimeManagement.Api.Tests/` xUnit test project (`dotnet new xunit`)
- [x] 1.2 Add `<ProjectReference>` to `TimeManagement.Api` in the test project
- [x] 1.3 Add `Microsoft.EntityFrameworkCore.InMemory` package to the test project
- [x] 1.4 Add the test project to `TimeManagement.sln`

## 2. Test Infrastructure

- [x] 2.1 Create a helper method (or factory) that returns a fresh `AppDbContext` backed by a uniquely-named in-memory database for each test

## 3. GetAllAsync Tests

- [x] 3.1 Test: `GetAllAsync_WithRoles_ReturnsAllRoleDtos` — seed two roles, assert both are returned with correct fields
- [x] 3.2 Test: `GetAllAsync_Empty_ReturnsEmptyList` — empty database, assert result is an empty list

## 4. GetByIdAsync Tests

- [x] 4.1 Test: `GetByIdAsync_ExistingId_ReturnsRoleDto` — seed a role, assert correct dto returned
- [x] 4.2 Test: `GetByIdAsync_UnknownId_ReturnsNull` — call with random Guid, assert null

## 5. CreateAsync Tests

- [x] 5.1 Test: `CreateAsync_ValidRequest_ReturnsSuccess` — assert `IsSuccess = true` and dto has correct name and non-empty Id
- [x] 5.2 Test: `CreateAsync_ValidRequest_PersistsToDatabase` — assert role present in context after create
- [x] 5.3 Test: `CreateAsync_DuplicateName_ReturnsConflict` — seed a role, create with same name (different case), assert `Status = Conflict`

## 6. UpdateAsync Tests

- [x] 6.1 Test: `UpdateAsync_ValidRequest_ReturnsSuccess` — seed a role, update name, assert `IsSuccess = true` and updated dto
- [x] 6.2 Test: `UpdateAsync_ValidRequest_PersistsToDatabase` — assert role in DB reflects updated values
- [x] 6.3 Test: `UpdateAsync_UnknownId_ReturnsNotFound` — assert `Status = NotFound`
- [x] 6.4 Test: `UpdateAsync_NameTakenByOtherRole_ReturnsConflict` — seed two roles, update first with second's name, assert `Status = Conflict`

## 7. DeleteAsync Tests

- [x] 7.1 Test: `DeleteAsync_UnassignedRole_ReturnsNoContent` — assert `Status = NoContent`
- [x] 7.2 Test: `DeleteAsync_UnassignedRole_RemovesFromDatabase` — assert role no longer in DB after delete
- [x] 7.3 Test: `DeleteAsync_UnknownId_ReturnsNotFound` — assert `Status = NotFound`
- [x] 7.4 Test: `DeleteAsync_AssignedRole_ReturnsConflict` — seed role + EmployeeRole assignment, assert `Status = Conflict`

## 8. Verify

- [x] 8.1 Run `dotnet test` and confirm all tests pass with 0 failures
