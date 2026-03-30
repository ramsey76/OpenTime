## ADDED Requirements

### Requirement: Role definition
The system SHALL maintain a `Roles` table that defines the set of application-level authorization roles. Each role SHALL have a unique name.

Fields stored per role:
- `Id` — application-generated GUID primary key
- `Name` — string, max 50 chars, unique
- `Description` — string, max 500 chars

#### Scenario: Role is stored with required fields
- **WHEN** a role record is persisted to the database
- **THEN** the record SHALL contain a non-empty Id and Name

#### Scenario: Role names are unique
- **WHEN** two role records exist in the database
- **THEN** each SHALL have a distinct Name value

### Requirement: Employee role assignment
The system SHALL support assigning one or more roles to an employee via an `EmployeeRoles` join table. An employee MAY have zero or more roles. A role MAY be assigned to multiple employees.

Fields stored per assignment:
- `EmployeeId` — foreign key to `Employees` (part of composite PK)
- `RoleId` — foreign key to `Roles` (part of composite PK)

#### Scenario: Employee is assigned a role
- **WHEN** an `EmployeeRoles` record is created with a valid EmployeeId and RoleId
- **THEN** the record SHALL be persisted and the employee SHALL be considered to hold that role

#### Scenario: Duplicate role assignment is rejected
- **WHEN** an `EmployeeRoles` record is created with a EmployeeId and RoleId combination that already exists
- **THEN** the database SHALL reject the record due to a primary key constraint violation

#### Scenario: Employee can hold multiple roles
- **WHEN** multiple `EmployeeRoles` records exist with the same EmployeeId and different RoleIds
- **THEN** all records SHALL be retrievable and the employee SHALL be considered to hold each assigned role
