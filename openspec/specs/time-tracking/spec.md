## ADDED Requirements

### Requirement: Time entry recording
The system SHALL maintain a `TimeEntries` table to record hours logged by employees against projects. Each time entry SHALL reference a valid employee and project.

Fields stored per time entry:
- `Id` — application-generated GUID primary key
- `EmployeeId` — foreign key to `Employees`
- `ProjectId` — foreign key to `Projects`
- `Date` — DateOnly (the date the work was performed)
- `Hours` — decimal (4,2) — hours worked, must be greater than zero
- `Description` — string, max 500 chars

#### Scenario: Time entry is created with required fields
- **WHEN** a time entry record is persisted to the database
- **THEN** the record SHALL contain a non-empty Id, EmployeeId, ProjectId, Date, and a positive Hours value

#### Scenario: Time entry references a valid employee and project
- **WHEN** a time entry record is saved
- **THEN** both EmployeeId and ProjectId SHALL match existing records in their respective tables

### Requirement: Time entry is scoped to assigned projects
An employee SHALL only be able to create time entries for projects they are assigned to via `ProjectAssignments`. The data model SHALL enforce this relationship.

#### Scenario: Time entry employee and project match a project assignment
- **WHEN** a time entry is created with a given EmployeeId and ProjectId
- **THEN** a corresponding `ProjectAssignments` record with that EmployeeId and ProjectId SHALL exist

#### Scenario: Multiple time entries can exist for the same employee and project
- **WHEN** multiple time entries are created with the same EmployeeId and ProjectId on different dates
- **THEN** all records SHALL be valid and independently retrievable

#### Scenario: Time entries for a deleted project are rejected
- **WHEN** a time entry is created referencing a ProjectId that does not exist
- **THEN** the database SHALL reject the record due to a foreign key constraint
