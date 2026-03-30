## ADDED Requirements

### Requirement: List all employees
The system SHALL provide an endpoint that returns all employee records.

#### Scenario: Employees exist
- **WHEN** a GET request is made to `/api/employees`
- **THEN** the system returns `200 OK` with an array of employee objects containing `id`, `externalId`, `firstName`, `lastName`, `email`, and `departmentId`

#### Scenario: No employees exist
- **WHEN** a GET request is made to `/api/employees` and no employees have been created
- **THEN** the system returns `200 OK` with an empty array

### Requirement: Get an employee by ID
The system SHALL provide an endpoint that returns a single employee by their identifier.

#### Scenario: Employee found
- **WHEN** a GET request is made to `/api/employees/{id}` with a valid existing ID
- **THEN** the system returns `200 OK` with the employee object containing `id`, `externalId`, `firstName`, `lastName`, `email`, and `departmentId`

#### Scenario: Employee not found
- **WHEN** a GET request is made to `/api/employees/{id}` with an ID that does not exist
- **THEN** the system returns `404 Not Found`

### Requirement: Create an employee
The system SHALL provide an endpoint that creates a new employee record.

#### Scenario: Successful creation
- **WHEN** a POST request is made to `/api/employees` with a valid `externalId`, `firstName`, `lastName`, `email`, and an existing `departmentId`
- **THEN** the system persists the employee and returns `201 Created` with the created employee object and a `Location` header pointing to `/api/employees/{id}`

#### Scenario: Duplicate ExternalId
- **WHEN** a POST request is made to `/api/employees` with an `externalId` that already exists
- **THEN** the system returns `409 Conflict`

#### Scenario: Department does not exist
- **WHEN** a POST request is made to `/api/employees` with a `departmentId` that does not match any existing department
- **THEN** the system returns `422 Unprocessable Entity`

#### Scenario: Missing required field
- **WHEN** a POST request is made to `/api/employees` without one of `externalId`, `firstName`, `lastName`, `email`, or `departmentId`
- **THEN** the system returns `400 Bad Request`

#### Scenario: FirstName exceeds maximum length
- **WHEN** a POST request is made to `/api/employees` with a `firstName` longer than 100 characters
- **THEN** the system returns `400 Bad Request`

#### Scenario: LastName exceeds maximum length
- **WHEN** a POST request is made to `/api/employees` with a `lastName` longer than 100 characters
- **THEN** the system returns `400 Bad Request`

#### Scenario: Email exceeds maximum length
- **WHEN** a POST request is made to `/api/employees` with an `email` longer than 256 characters
- **THEN** the system returns `400 Bad Request`

#### Scenario: ExternalId exceeds maximum length
- **WHEN** a POST request is made to `/api/employees` with an `externalId` longer than 128 characters
- **THEN** the system returns `400 Bad Request`

### Requirement: Update an employee
The system SHALL provide an endpoint that updates an existing employee's fields.

#### Scenario: Successful update
- **WHEN** a PUT request is made to `/api/employees/{id}` with valid `firstName`, `lastName`, `email`, and `departmentId`
- **THEN** the system updates the employee and returns `200 OK` with the updated employee object

#### Scenario: Employee not found
- **WHEN** a PUT request is made to `/api/employees/{id}` with an ID that does not exist
- **THEN** the system returns `404 Not Found`

#### Scenario: Department does not exist on update
- **WHEN** a PUT request is made to `/api/employees/{id}` with a `departmentId` that does not match any existing department
- **THEN** the system returns `422 Unprocessable Entity`

#### Scenario: Missing required field on update
- **WHEN** a PUT request is made to `/api/employees/{id}` without one of `firstName`, `lastName`, `email`, or `departmentId`
- **THEN** the system returns `400 Bad Request`

### Requirement: Delete an employee
The system SHALL provide an endpoint that deletes an employee by their identifier.

#### Scenario: Successful deletion
- **WHEN** a DELETE request is made to `/api/employees/{id}` for an existing employee
- **THEN** the system deletes the employee and returns `204 No Content`

#### Scenario: Employee not found
- **WHEN** a DELETE request is made to `/api/employees/{id}` with an ID that does not exist
- **THEN** the system returns `404 Not Found`
