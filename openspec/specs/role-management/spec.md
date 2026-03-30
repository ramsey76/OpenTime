# Role Management

## Purpose

TBD

## Requirements

### Requirement: List all roles
The system SHALL provide an endpoint that returns all roles.

#### Scenario: Roles exist
- **WHEN** a GET request is made to `/api/roles`
- **THEN** the system returns `200 OK` with an array of role objects containing `id`, `name`, and `description`

#### Scenario: No roles exist
- **WHEN** a GET request is made to `/api/roles` and no roles have been created
- **THEN** the system returns `200 OK` with an empty array

### Requirement: Get a role by ID
The system SHALL provide an endpoint that returns a single role by its identifier.

#### Scenario: Role found
- **WHEN** a GET request is made to `/api/roles/{id}` with a valid existing ID
- **THEN** the system returns `200 OK` with the role object containing `id`, `name`, and `description`

#### Scenario: Role not found
- **WHEN** a GET request is made to `/api/roles/{id}` with an ID that does not exist
- **THEN** the system returns `404 Not Found`

### Requirement: Create a role
The system SHALL provide an endpoint that creates a new role.

#### Scenario: Successful creation
- **WHEN** a POST request is made to `/api/roles` with a valid `name` (and optional `description`)
- **THEN** the system persists the role and returns `201 Created` with the created role object and a `Location` header pointing to `/api/roles/{id}`

#### Scenario: Duplicate name
- **WHEN** a POST request is made to `/api/roles` with a `name` that already exists (case-insensitive)
- **THEN** the system returns `409 Conflict`

#### Scenario: Missing name
- **WHEN** a POST request is made to `/api/roles` without a `name`
- **THEN** the system returns `400 Bad Request`

#### Scenario: Name exceeds maximum length
- **WHEN** a POST request is made to `/api/roles` with a `name` longer than 50 characters
- **THEN** the system returns `400 Bad Request`

### Requirement: Update a role
The system SHALL provide an endpoint that updates an existing role's name and description.

#### Scenario: Successful update
- **WHEN** a PUT request is made to `/api/roles/{id}` with a valid `name` (and optional `description`)
- **THEN** the system updates the role and returns `200 OK` with the updated role object

#### Scenario: Role not found
- **WHEN** a PUT request is made to `/api/roles/{id}` with an ID that does not exist
- **THEN** the system returns `404 Not Found`

#### Scenario: Duplicate name on update
- **WHEN** a PUT request is made to `/api/roles/{id}` with a `name` already used by a different role
- **THEN** the system returns `409 Conflict`

#### Scenario: Missing name
- **WHEN** a PUT request is made to `/api/roles/{id}` without a `name`
- **THEN** the system returns `400 Bad Request`

### Requirement: Delete a role
The system SHALL provide an endpoint that deletes a role by its identifier.

#### Scenario: Successful deletion
- **WHEN** a DELETE request is made to `/api/roles/{id}` for a role not assigned to any employee
- **THEN** the system deletes the role and returns `204 No Content`

#### Scenario: Role not found
- **WHEN** a DELETE request is made to `/api/roles/{id}` with an ID that does not exist
- **THEN** the system returns `404 Not Found`

#### Scenario: Role assigned to employees
- **WHEN** a DELETE request is made to `/api/roles/{id}` for a role that is currently assigned to one or more employees
- **THEN** the system returns `409 Conflict` with a message indicating the role is in use
