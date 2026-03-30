## ADDED Requirements

### Requirement: Department record storage
The system SHALL maintain a `Departments` table to represent organizational units that employees belong to. Each department SHALL have a unique code.

Fields stored per department:
- `Id` — application-generated GUID primary key
- `Name` — string, max 100 chars
- `Code` — string, max 10 chars, unique

#### Scenario: Department record is created with required fields
- **WHEN** a department record is persisted to the database
- **THEN** the record SHALL contain a non-empty Id, Name, and Code

#### Scenario: Department codes are unique
- **WHEN** two department records exist in the database
- **THEN** each SHALL have a distinct Code value

### Requirement: Department groups employees
The system SHALL support associating multiple employees with a single department via the `DepartmentId` foreign key on `Employees`. A department MAY have zero or more employees.

#### Scenario: Multiple employees can belong to the same department
- **WHEN** two employee records reference the same DepartmentId
- **THEN** both records SHALL be valid and the department SHALL be retrievable with both employees

#### Scenario: Department deletion is blocked while employees are assigned
- **WHEN** a department record is deleted and one or more employees reference its Id
- **THEN** the database SHALL reject the deletion due to a foreign key constraint
