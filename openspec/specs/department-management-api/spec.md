## Purpose

This capability defines the HTTP API for managing departments. It covers listing, retrieving, creating, updating, and deleting department resources via REST endpoints.

## Requirements

### Requirement: List all departments
The system SHALL provide an endpoint that returns all departments.

#### Scenario: Departments exist
- **WHEN** a GET request is made to `/api/departments`
- **THEN** the system returns `200 OK` with an array of department objects containing `id`, `name`, `code`, and `parentDepartmentId` (null for root departments)

#### Scenario: No departments exist
- **WHEN** a GET request is made to `/api/departments` and no departments have been created
- **THEN** the system returns `200 OK` with an empty array

### Requirement: Get a department by ID
The system SHALL provide an endpoint that returns a single department by its identifier.

#### Scenario: Department found
- **WHEN** a GET request is made to `/api/departments/{id}` with a valid existing ID
- **THEN** the system returns `200 OK` with the department object containing `id`, `name`, `code`, and `parentDepartmentId`

#### Scenario: Department not found
- **WHEN** a GET request is made to `/api/departments/{id}` with an ID that does not exist
- **THEN** the system returns `404 Not Found`

### Requirement: Create a department
The system SHALL provide an endpoint that creates a new department.

#### Scenario: Successful creation without parent
- **WHEN** a POST request is made to `/api/departments` with a valid `name` and `code` and no `parentDepartmentId`
- **THEN** the system persists the department and returns `201 Created` with the created department object (parentDepartmentId null) and a `Location` header

#### Scenario: Successful creation with parent
- **WHEN** a POST request is made to `/api/departments` with a valid `name`, `code`, and an existing `parentDepartmentId`
- **THEN** the system persists the department and returns `201 Created` with the created department object including the parentDepartmentId

#### Scenario: Duplicate code
- **WHEN** a POST request is made to `/api/departments` with a `code` that already exists (case-insensitive)
- **THEN** the system returns `409 Conflict`

#### Scenario: Parent department does not exist
- **WHEN** a POST request is made to `/api/departments` with a `parentDepartmentId` that does not match any existing department
- **THEN** the system returns `422 Unprocessable Entity`

#### Scenario: Missing name
- **WHEN** a POST request is made to `/api/departments` without a `name`
- **THEN** the system returns `400 Bad Request`

#### Scenario: Missing code
- **WHEN** a POST request is made to `/api/departments` without a `code`
- **THEN** the system returns `400 Bad Request`

#### Scenario: Name exceeds maximum length
- **WHEN** a POST request is made to `/api/departments` with a `name` longer than 100 characters
- **THEN** the system returns `400 Bad Request`

#### Scenario: Code exceeds maximum length
- **WHEN** a POST request is made to `/api/departments` with a `code` longer than 10 characters
- **THEN** the system returns `400 Bad Request`

### Requirement: Update a department
The system SHALL provide an endpoint that updates an existing department's name, code, and parent.

#### Scenario: Successful update without changing parent
- **WHEN** a PUT request is made to `/api/departments/{id}` with a valid `name` and `code`
- **THEN** the system updates the department and returns `200 OK` with the updated department object

#### Scenario: Successful update assigning a parent
- **WHEN** a PUT request is made to `/api/departments/{id}` with a valid `parentDepartmentId`
- **THEN** the system updates the department and returns `200 OK` with the updated object including parentDepartmentId

#### Scenario: Successful update removing a parent
- **WHEN** a PUT request is made to `/api/departments/{id}` with `parentDepartmentId` explicitly set to null
- **THEN** the system updates the department to be a root department and returns `200 OK`

#### Scenario: Department not found
- **WHEN** a PUT request is made to `/api/departments/{id}` with an ID that does not exist
- **THEN** the system returns `404 Not Found`

#### Scenario: Duplicate code on update
- **WHEN** a PUT request is made to `/api/departments/{id}` with a `code` already used by a different department
- **THEN** the system returns `409 Conflict`

#### Scenario: Parent department does not exist on update
- **WHEN** a PUT request is made to `/api/departments/{id}` with a `parentDepartmentId` that does not match any existing department
- **THEN** the system returns `422 Unprocessable Entity`

#### Scenario: Circular reference on update
- **WHEN** a PUT request is made to `/api/departments/{id}` with a `parentDepartmentId` that would create a cycle in the hierarchy
- **THEN** the system returns `422 Unprocessable Entity`

#### Scenario: Missing name
- **WHEN** a PUT request is made to `/api/departments/{id}` without a `name`
- **THEN** the system returns `400 Bad Request`

### Requirement: Delete a department
The system SHALL provide an endpoint that deletes a department by its identifier.

#### Scenario: Successful deletion
- **WHEN** a DELETE request is made to `/api/departments/{id}` for a department with no assigned employees and no subdepartments
- **THEN** the system deletes the department and returns `204 No Content`

#### Scenario: Department not found
- **WHEN** a DELETE request is made to `/api/departments/{id}` with an ID that does not exist
- **THEN** the system returns `404 Not Found`

#### Scenario: Department has assigned employees
- **WHEN** a DELETE request is made to `/api/departments/{id}` for a department that has one or more employees assigned
- **THEN** the system returns `409 Conflict` with a message indicating the department is in use

#### Scenario: Department has subdepartments
- **WHEN** a DELETE request is made to `/api/departments/{id}` for a department that has one or more subdepartments
- **THEN** the system returns `409 Conflict` with a message indicating the department has subdepartments
