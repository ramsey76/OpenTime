## ADDED Requirements

### Requirement: List all departments
The system SHALL provide an endpoint that returns all departments.

#### Scenario: Departments exist
- **WHEN** a GET request is made to `/api/departments`
- **THEN** the system returns `200 OK` with an array of department objects containing `id`, `name`, and `code`

#### Scenario: No departments exist
- **WHEN** a GET request is made to `/api/departments` and no departments have been created
- **THEN** the system returns `200 OK` with an empty array

### Requirement: Get a department by ID
The system SHALL provide an endpoint that returns a single department by its identifier.

#### Scenario: Department found
- **WHEN** a GET request is made to `/api/departments/{id}` with a valid existing ID
- **THEN** the system returns `200 OK` with the department object containing `id`, `name`, and `code`

#### Scenario: Department not found
- **WHEN** a GET request is made to `/api/departments/{id}` with an ID that does not exist
- **THEN** the system returns `404 Not Found`

### Requirement: Create a department
The system SHALL provide an endpoint that creates a new department.

#### Scenario: Successful creation
- **WHEN** a POST request is made to `/api/departments` with a valid `name` and `code`
- **THEN** the system persists the department and returns `201 Created` with the created department object and a `Location` header pointing to `/api/departments/{id}`

#### Scenario: Duplicate code
- **WHEN** a POST request is made to `/api/departments` with a `code` that already exists (case-insensitive)
- **THEN** the system returns `409 Conflict`

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
The system SHALL provide an endpoint that updates an existing department's name and code.

#### Scenario: Successful update
- **WHEN** a PUT request is made to `/api/departments/{id}` with a valid `name` and `code`
- **THEN** the system updates the department and returns `200 OK` with the updated department object

#### Scenario: Department not found
- **WHEN** a PUT request is made to `/api/departments/{id}` with an ID that does not exist
- **THEN** the system returns `404 Not Found`

#### Scenario: Duplicate code on update
- **WHEN** a PUT request is made to `/api/departments/{id}` with a `code` already used by a different department
- **THEN** the system returns `409 Conflict`

#### Scenario: Missing name
- **WHEN** a PUT request is made to `/api/departments/{id}` without a `name`
- **THEN** the system returns `400 Bad Request`

### Requirement: Delete a department
The system SHALL provide an endpoint that deletes a department by its identifier.

#### Scenario: Successful deletion
- **WHEN** a DELETE request is made to `/api/departments/{id}` for a department with no assigned employees
- **THEN** the system deletes the department and returns `204 No Content`

#### Scenario: Department not found
- **WHEN** a DELETE request is made to `/api/departments/{id}` with an ID that does not exist
- **THEN** the system returns `404 Not Found`

#### Scenario: Department has assigned employees
- **WHEN** a DELETE request is made to `/api/departments/{id}` for a department that has one or more employees assigned
- **THEN** the system returns `409 Conflict` with a message indicating the department is in use
