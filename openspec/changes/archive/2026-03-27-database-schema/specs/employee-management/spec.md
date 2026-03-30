## ADDED Requirements

### Requirement: Employee record storage
The system SHALL maintain an employee record for each user, storing identity fields linked to their Entra ID account. No authentication credentials SHALL be stored — only the stable Entra ID object identifier and display information.

Fields stored per employee:
- `Id` — application-generated GUID primary key
- `ExternalId` — Entra ID object identifier (string, max 128 chars, unique)
- `FirstName` — string, max 100 chars
- `LastName` — string, max 100 chars
- `Email` — string, max 256 chars
- `DepartmentId` — foreign key to `Departments`

#### Scenario: Employee record is created with required fields
- **WHEN** a new employee record is persisted to the database
- **THEN** the record SHALL contain a non-empty Id, ExternalId, FirstName, LastName, Email, and DepartmentId

#### Scenario: ExternalId is unique across all employees
- **WHEN** two employee records are created
- **THEN** each SHALL have a distinct ExternalId value

### Requirement: Employee belongs to exactly one department
Each employee record SHALL reference exactly one department via a non-nullable `DepartmentId` foreign key.

#### Scenario: Employee record references a valid department
- **WHEN** an employee record is saved
- **THEN** the `DepartmentId` SHALL match an existing `Departments.Id` value

#### Scenario: Employee record cannot exist without a department
- **WHEN** an employee record is created without a DepartmentId
- **THEN** the database SHALL reject the record due to a NOT NULL constraint
