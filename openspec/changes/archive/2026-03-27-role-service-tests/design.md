## Context

`RoleService` is the business logic layer for the Roles API, established by the `roles-api` change. The `api-minimal` skill designates the service layer as the primary unit-test seam and lists five coverage areas: success paths, not-found paths, validation failures, duplicate checks, and state changes. No test project exists in the solution yet — this change creates it.

## Goals / Non-Goals

**Goals:**
- Create a `TimeManagement.Api.Tests` xUnit project
- Unit test all five `RoleService` methods against an EF Core in-memory database
- Establish the test project structure and conventions for future service tests

**Non-Goals:**
- Integration tests (no real database)
- Handler/endpoint tests
- Testing other services (out of scope for this change)

## Decisions

### 1. xUnit as the test framework

**Decision:** Use xUnit.

**Rationale:** xUnit is the standard for .NET projects and integrates well with `dotnet test`. No alternatives considered — xUnit is the de facto choice.

### 2. EF Core in-memory provider for test isolation

**Decision:** Use `Microsoft.EntityFrameworkCore.InMemory` to provide a fresh `AppDbContext` per test.

**Rationale:** The `api-minimal` skill explicitly recommends `UseInMemoryDatabase` for service tests. It keeps tests fast, avoids external dependencies, and means each test starts with a clean state. Each test method gets its own named in-memory database to prevent cross-test contamination.

**Alternatives considered:**
- SQLite in-memory — more faithful to PostgreSQL behaviour, but adds complexity for no benefit at this coverage level.
- Mocking `AppDbContext` — rejected; EF Core DbSets are hard to mock cleanly and the skill recommends in-memory over mocks.

### 3. Test project location: `src/TimeManagement.Api.Tests/`

**Decision:** Place the test project under `src/` alongside the source projects.

**Rationale:** The `api-minimal` skill explicitly states: "If tests live alongside source projects in the solution, keep the test projects under `src/`, not inside the API project folder."

### 4. One test class per service method

**Decision:** Organise tests into a single `RoleServiceTests` class with clearly named test methods per scenario.

**Rationale:** All tests share the same subject (`RoleService`) and setup pattern. A flat structure with descriptive method names (e.g. `CreateAsync_DuplicateName_ReturnsConflict`) is easier to scan than multiple nested classes.

### 5. Project reference, not package reference

**Decision:** The test project references `TimeManagement.Api` directly via `<ProjectReference>`.

**Rationale:** `RoleService` and `AppDbContext` live in the API project — a project reference is the only way to access them. No extra abstraction is needed.

## Risks / Trade-offs

- **In-memory vs real DB behaviour** → The in-memory provider does not enforce unique indexes (e.g. the unique index on `Role.Name`). Duplicate-name checks must be tested via the service logic, not the DB constraint. Mitigation: the service already performs explicit duplicate checks before saving — these are what the tests exercise.
- **First test project** → Naming and structural choices here will be copied for future test projects. Mitigation: follow the skill's guidance exactly.
