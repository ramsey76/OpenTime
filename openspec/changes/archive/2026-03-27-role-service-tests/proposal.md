## Why

`RoleService` was introduced as the primary business logic layer for the Roles API, but has no automated tests. The `api-minimal` skill mandates the service layer as the main unit-test seam — adding tests now ensures correctness, prevents regressions, and establishes the test pattern for future services.

## What Changes

- New `TimeManagement.Api.Tests` test project added to the solution.
- Unit tests for `RoleService` covering all five operations: `GetAllAsync`, `GetByIdAsync`, `CreateAsync`, `UpdateAsync`, `DeleteAsync`.
- EF Core in-memory provider used to keep tests fast and self-contained.

## Capabilities

### New Capabilities

- `role-service-testing`: Unit test coverage for `RoleService` — success paths, not-found paths, duplicate-name conflicts, and delete-with-assignments conflict.

### Modified Capabilities

_(none)_

## Impact

- **New project:** `src/TimeManagement.Api.Tests/` (xUnit test project)
- **Solution file:** `TimeManagement.sln` updated with new test project reference
- **New dependency:** `Microsoft.EntityFrameworkCore.InMemory` (test project only)
- **No changes** to production code
