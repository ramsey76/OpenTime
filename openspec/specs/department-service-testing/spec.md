## Purpose

This capability defines the unit test coverage requirements for `DepartmentService`. It specifies the scenarios that must be verified for each service method: `GetAllAsync`, `GetByIdAsync`, `CreateAsync`, `UpdateAsync`, and `DeleteAsync`.

## Requirements

### Requirement: GetAllAsync is covered by tests
The test suite SHALL verify that `DepartmentService.GetAllAsync` returns the correct data.

#### Scenario: Returns all departments when departments exist
- **WHEN** departments exist in the database and `GetAllAsync` is called
- **THEN** it returns a list containing a `DepartmentDto` for each department with correct `Id`, `Name`, and `Code`

#### Scenario: Returns empty list when no departments exist
- **WHEN** the database contains no departments and `GetAllAsync` is called
- **THEN** it returns an empty list

### Requirement: GetByIdAsync is covered by tests
The test suite SHALL verify that `DepartmentService.GetByIdAsync` returns the correct result by ID.

#### Scenario: Returns department when ID exists
- **WHEN** a department with a known ID exists and `GetByIdAsync` is called with that ID
- **THEN** it returns a `DepartmentDto` with matching `Id`, `Name`, and `Code`

#### Scenario: Returns null when ID does not exist
- **WHEN** no department with the given ID exists and `GetByIdAsync` is called
- **THEN** it returns `null`

### Requirement: CreateAsync is covered by tests
The test suite SHALL verify that `DepartmentService.CreateAsync` correctly creates departments and enforces constraints.

#### Scenario: Creates department and returns success result
- **WHEN** `CreateAsync` is called with a valid name and code that do not already exist
- **THEN** it returns a result with `IsSuccess = true` and a `DepartmentDto` with the correct `Name`, `Code`, and a non-empty `Id`

#### Scenario: Persists the department to the database
- **WHEN** `CreateAsync` is called with a valid name and code
- **THEN** the department is present in the database after the call

#### Scenario: Returns conflict when code already exists
- **WHEN** a department with the same code (case-insensitive) already exists and `CreateAsync` is called
- **THEN** it returns a result with `Status = DepartmentResultStatus.Conflict`

### Requirement: UpdateAsync is covered by tests
The test suite SHALL verify that `DepartmentService.UpdateAsync` correctly updates departments and enforces constraints.

#### Scenario: Updates department and returns success result
- **WHEN** a department exists and `UpdateAsync` is called with a new valid name and code
- **THEN** it returns a result with `IsSuccess = true` and a `DepartmentDto` reflecting the updated values

#### Scenario: Persists the update to the database
- **WHEN** `UpdateAsync` is called with new values for an existing department
- **THEN** the department in the database reflects the updated name and code

#### Scenario: Returns not found when ID does not exist
- **WHEN** no department with the given ID exists and `UpdateAsync` is called
- **THEN** it returns a result with `Status = DepartmentResultStatus.NotFound`

#### Scenario: Returns conflict when new code belongs to another department
- **WHEN** a different department already uses the requested code and `UpdateAsync` is called
- **THEN** it returns a result with `Status = DepartmentResultStatus.Conflict`

### Requirement: DeleteAsync is covered by tests
The test suite SHALL verify that `DepartmentService.DeleteAsync` correctly deletes departments and enforces constraints.

#### Scenario: Deletes department and returns no-content result
- **WHEN** a department exists with no employee assignments and `DeleteAsync` is called
- **THEN** it returns a result with `Status = DepartmentResultStatus.NoContent`

#### Scenario: Removes the department from the database
- **WHEN** `DeleteAsync` is called for an existing unassigned department
- **THEN** the department is no longer present in the database after the call

#### Scenario: Returns not found when ID does not exist
- **WHEN** no department with the given ID exists and `DeleteAsync` is called
- **THEN** it returns a result with `Status = DepartmentResultStatus.NotFound`

#### Scenario: Returns conflict when department has assigned employees
- **WHEN** a department has one or more employees assigned and `DeleteAsync` is called
- **THEN** it returns a result with `Status = DepartmentResultStatus.Conflict`
