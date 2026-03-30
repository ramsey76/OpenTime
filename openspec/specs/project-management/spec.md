## ADDED Requirements

### Requirement: Project record storage
The system SHALL maintain a `Projects` table to represent trackable work items that employees can log time against. Each project SHALL have a unique code.

Fields stored per project:
- `Id` — application-generated GUID primary key
- `Name` — string, max 200 chars
- `Code` — string, max 10 chars, unique
- `Status` — string, max 20 chars (e.g. "Active", "Archived")
- `Description` — string, max 1000 chars

#### Scenario: Project record is created with required fields
- **WHEN** a project record is persisted to the database
- **THEN** the record SHALL contain a non-empty Id, Name, Code, and Status

#### Scenario: Project codes are unique
- **WHEN** two project records exist in the database
- **THEN** each SHALL have a distinct Code value

### Requirement: Employee project assignment
The system SHALL maintain a `ProjectAssignments` join table to control which employees can log time against which projects. An employee MUST be assigned to a project before logging time against it.

Fields stored per assignment:
- `EmployeeId` — foreign key to `Employees` (part of composite PK)
- `ProjectId` — foreign key to `Projects` (part of composite PK)

#### Scenario: Employee is assigned to a project
- **WHEN** a `ProjectAssignments` record is created with a valid EmployeeId and ProjectId
- **THEN** the record SHALL be persisted and the employee SHALL be considered assigned to that project

#### Scenario: Duplicate assignment is rejected
- **WHEN** a `ProjectAssignments` record is created with an EmployeeId and ProjectId combination that already exists
- **THEN** the database SHALL reject the record due to a primary key constraint violation

#### Scenario: Employee can be assigned to multiple projects
- **WHEN** multiple `ProjectAssignments` records exist with the same EmployeeId and different ProjectIds
- **THEN** all records SHALL be retrievable and the employee SHALL be considered assigned to each project
