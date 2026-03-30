## Context

The `Department` entity currently has `Id`, `Name`, and `Code`. This change adds an optional `ParentDepartmentId` self-referencing FK, enabling a tree structure. The existing API, DTOs, and service layer all need updating. A DB migration is required. Three integrity rules must be enforced in the service layer and tested.

## Goals / Non-Goals

**Goals:**
- Optional parent on department (flat departments continue to work unchanged)
- Three service-enforced integrity rules: single parent (structural), parent must exist, no circular references
- Delete blocked when subdepartments exist
- API exposes `parentDepartmentId` on all read/write endpoints
- Unit tests for all new rules

**Non-Goals:**
- Returning a nested tree structure from the API (flat list with `parentDepartmentId` is sufficient)
- Maximum depth enforcement
- Moving an entire subtree when a parent is reassigned
- Cascading delete of subdepartments

## Decisions

### 1. Self-referencing nullable FK on `Department`

**Decision:** Add `ParentDepartmentId Guid?` to `Department` with a self-referencing FK configured as `DeleteBehavior.Restrict`.

**Rationale:** Nullable means root departments need no change. `Restrict` prevents deleting a parent that still has children — this is enforced at both the service layer (better error message) and the DB (safety net).

**Alternatives considered:**
- Separate `DepartmentHierarchy` join table — rejected, adds complexity for a simple parent pointer.
- Adjacency list with `Depth` column — rejected, not needed without depth constraints.

### 2. Circular reference detection: ancestor walk in the service

**Decision:** When setting a parent, walk up the ancestor chain of the proposed parent and reject if the current department's ID appears.

**Rationale:** The data set is small (departments are organisational units, not unbounded). An in-memory ancestor walk via EF queries is simple and correct. No need for recursive CTEs or graph algorithms.

**Algorithm:**
```
visited = {}
current = proposedParentId
while current != null:
    if current == departmentBeingUpdated.Id → reject (cycle)
    if current in visited → break (already checked this branch)
    visited.add(current)
    current = Departments.Find(current).ParentDepartmentId
```

**Alternatives considered:**
- Recursive SQL CTE — correct but couples service to DB dialect.
- Depth limit as a proxy for cycles — rejected, doesn't actually prevent cycles.

### 3. HTTP 422 Unprocessable Entity for integrity rule violations

**Decision:** Return `422 Unprocessable Entity` for parent-not-found and circular-reference violations. Continue using `409 Conflict` for duplicate code and subdepartment-blocks-delete.

**Rationale:** 422 is semantically correct for "the request is well-formed but violates a business rule" (RFC 9110). It distinguishes integrity violations from resource conflicts, making client error handling clearer.

### 4. Flat API response (parentDepartmentId field, not nested children)

**Decision:** `DepartmentDto` adds a nullable `ParentDepartmentId` field. The list endpoint returns a flat array. Clients build trees client-side if needed.

**Rationale:** A nested tree response requires recursive projection which adds complexity and can cause N+1 queries. The flat list is simple, performs well, and is sufficient for the stated use case.

### 5. `DepartmentResult` extended with `UnprocessableEntity` status

**Decision:** Add `UnprocessableEntity` to `DepartmentResultStatus` for the two new validation failures.

**Rationale:** Reusing `Conflict` for both duplicate-code and circular-reference would conflate two distinct error categories. A separate status keeps handler mapping clean.

## Risks / Trade-offs

- **Ancestor walk is O(depth) queries** → For realistic org structures (< 20 levels) this is negligible. If depth grows unbounded, consider caching or a materialized path. Mitigation: acceptable for now.
- **In-memory provider ignores FK constraints** → Unit tests for the delete guard must check service logic, not DB enforcement (same pattern as employee/role assignments). Mitigation: explicit `AnyAsync` check in `DeleteAsync`.
- **Self-referencing FK + `Restrict`** → EF Core requires careful configuration to avoid cascade cycles warning. Mitigation: configure explicitly in `DepartmentConfiguration` with `OnDelete(DeleteBehavior.Restrict)`.

## Migration Plan

1. Add `ParentDepartmentId` column as nullable — no existing rows affected.
2. Run migration: `dotnet ef migrations add AddDepartmentParent`.
3. No data backfill needed — all existing departments remain root departments (null parent).
4. Rollback: remove migration and revert entity/configuration changes.
