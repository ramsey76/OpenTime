## Why

The application needs a way to manage roles (e.g. Developer, Manager, Designer) that can be assigned to employees. Currently the `Role` entity exists in the database but there is no API to create, read, update, or delete roles, making them unmanageable without direct database access.

## What Changes

- New REST API endpoints for role management: list all roles, get a role by ID, create a role, update a role, and delete a role.
- New `RolesController` in the API layer following the same patterns as other controllers in the project.
- New application-layer commands and queries (CQRS) for each operation.
- Input validation and appropriate HTTP status codes (404 for not found, 400 for validation errors, 409 for conflicts).

## Capabilities

### New Capabilities

- `role-management`: CRUD API for roles — create, read (single + list), update, and delete roles via REST endpoints.

### Modified Capabilities

_(none)_

## Impact

- **API**: New `RolesController` at `/api/roles`
- **Application layer**: New commands (`CreateRole`, `UpdateRole`, `DeleteRole`) and queries (`GetRoles`, `GetRoleById`)
- **Domain**: `Role` entity already exists — no domain changes required
- **Database**: No schema changes — `Roles` table already exists in the `timemanagement` schema
- **Dependencies**: No new packages required
