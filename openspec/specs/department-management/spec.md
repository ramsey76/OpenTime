## Requirements

### Requirement: Department record storage
The system SHALL maintain a `Departments` table to represent organizational units that employees belong to. Each department SHALL have a unique code. A department MAY optionally reference another department as its parent, forming a hierarchy.

Fields stored per department:
- `Id` — application-generated GUID primary key
- `Name` — string, max 100 chars
- `Code` — string, max 10 chars, unique
- `ParentDepartmentId` — nullable GUID foreign key referencing another department; null indicates a root department

Invariants:
- A department SHALL have at most one parent (enforced by the single nullable FK).
- A department SHALL NOT be its own ancestor (no circular references are permitted).

#### Scenario: Department record is created with required fields
- **WHEN** a department record is persisted to the database
- **THEN** the record SHALL contain a non-empty Id, Name, and Code

#### Scenario: Department codes are unique
- **WHEN** two department records exist in the database
- **THEN** each SHALL have a distinct Code value

#### Scenario: Root department has no parent
- **WHEN** a department is created without a ParentDepartmentId
- **THEN** its ParentDepartmentId SHALL be null

#### Scenario: Subdepartment references its parent
- **WHEN** a department is created with a valid ParentDepartmentId
- **THEN** its ParentDepartmentId SHALL match the Id of the parent department

### Requirement: Department groups employees
The system SHALL support associating multiple employees with a single department via the `DepartmentId` foreign key on `Employees`. A department MAY have zero or more employees.

#### Scenario: Multiple employees can belong to the same department
- **WHEN** two employee records reference the same DepartmentId
- **THEN** both records SHALL be valid and the department SHALL be retrievable with both employees

#### Scenario: Department deletion is blocked while employees are assigned
- **WHEN** a department record is deleted and one or more employees reference its Id
- **THEN** the database SHALL reject the deletion due to a foreign key constraint

### Requirement: Department hierarchy integrity
The system SHALL enforce structural integrity rules when assigning a parent to a department.

#### Scenario: Parent department must exist
- **WHEN** a department is created or updated with a non-null ParentDepartmentId that does not match any existing department
- **THEN** the operation SHALL be rejected

#### Scenario: Circular reference is rejected on create
- **WHEN** a department is created with a ParentDepartmentId that would make the new department an ancestor of itself
- **THEN** the operation SHALL be rejected

#### Scenario: Circular reference is rejected on update
- **WHEN** a department is updated with a ParentDepartmentId that would create a cycle in the hierarchy
- **THEN** the operation SHALL be rejected

#### Scenario: Department deletion is blocked while subdepartments exist
- **WHEN** a department record is deleted and one or more other departments reference its Id as their ParentDepartmentId
- **THEN** the operation SHALL be rejected
