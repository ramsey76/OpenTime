## ADDED Requirements

### Requirement: Departments list page with hierarchy
The system SHALL display a page at `/departments` that lists all departments in a table with columns for Name, Code, Parent, and Actions. Child departments SHALL be visually indented under their parent.

#### Scenario: Departments exist with hierarchy
- **WHEN** the user navigates to `/departments` and departments have parent-child relationships
- **THEN** the system SHALL fetch all departments from `GET /api/departments`, sort them into a hierarchy, and display them in a table with child departments indented beneath their parent

#### Scenario: Only root departments exist
- **WHEN** the user navigates to `/departments` and all departments have `parentDepartmentId` as null
- **THEN** the system SHALL display all departments at the same level without indentation

#### Scenario: No departments exist
- **WHEN** the user navigates to `/departments` and the API returns an empty array
- **THEN** the system SHALL display an empty state message indicating no departments have been created

#### Scenario: Loading state
- **WHEN** the user navigates to `/departments` and the API request is in progress
- **THEN** the system SHALL display a loading indicator

#### Scenario: API error on load
- **WHEN** the user navigates to `/departments` and the API request fails
- **THEN** the system SHALL display an error message

### Requirement: Create a department via modal form
The system SHALL provide a modal form to create a new department, triggered by an "Add Department" button on the departments page. The form SHALL include a name field, a code field, and an optional parent department dropdown.

#### Scenario: Successful creation without parent
- **WHEN** the user clicks "Add Department", fills in a valid name and code without selecting a parent, and submits
- **THEN** the system SHALL send a `POST /api/departments` request with `parentDepartmentId` as null, close the modal, and refresh the departments list

#### Scenario: Successful creation with parent
- **WHEN** the user clicks "Add Department", fills in a valid name and code, selects an existing department as parent, and submits
- **THEN** the system SHALL send a `POST /api/departments` request with the selected `parentDepartmentId`, close the modal, and refresh the departments list

#### Scenario: Name is required
- **WHEN** the user submits the create form without entering a name
- **THEN** the system SHALL display an inline validation error on the name field and SHALL NOT send an API request

#### Scenario: Code is required
- **WHEN** the user submits the create form without entering a code
- **THEN** the system SHALL display an inline validation error on the code field and SHALL NOT send an API request

#### Scenario: Name exceeds maximum length
- **WHEN** the user enters a name longer than 100 characters
- **THEN** the system SHALL display an inline validation error indicating the maximum length

#### Scenario: Code exceeds maximum length
- **WHEN** the user enters a code longer than 10 characters
- **THEN** the system SHALL display an inline validation error indicating the maximum length

#### Scenario: Duplicate code
- **WHEN** the user submits a code that already exists and the API returns `409 Conflict`
- **THEN** the system SHALL display a toast notification indicating the department code is already in use

#### Scenario: Cancel creation
- **WHEN** the user opens the create modal and clicks "Cancel" or closes the modal
- **THEN** the system SHALL close the modal without sending an API request

### Requirement: Edit a department via modal form
The system SHALL provide a modal form to edit an existing department, triggered by the "Edit" action on a department row. The form SHALL include name, code, and parent department dropdown pre-filled with current values.

#### Scenario: Modal opens pre-filled
- **WHEN** the user clicks "Edit" on a department row
- **THEN** the system SHALL open the department form modal pre-filled with the department's current name, code, and parent department selection

#### Scenario: Successful update
- **WHEN** the user modifies the department fields and submits
- **THEN** the system SHALL send a `PUT /api/departments/{id}` request, close the modal, and refresh the departments list

#### Scenario: Change parent department
- **WHEN** the user changes the parent department selection and submits
- **THEN** the system SHALL send a `PUT /api/departments/{id}` request with the new `parentDepartmentId` and refresh the hierarchical list

#### Scenario: Remove parent (make root)
- **WHEN** the user clears the parent department selection and submits
- **THEN** the system SHALL send a `PUT /api/departments/{id}` request with `parentDepartmentId` as null

#### Scenario: Duplicate code on update
- **WHEN** the user changes the code to one already used by another department and the API returns `409 Conflict`
- **THEN** the system SHALL display a toast notification indicating the department code is already in use

#### Scenario: Circular reference on update
- **WHEN** the user selects a parent that would create a circular reference and the API returns `422 Unprocessable Entity`
- **THEN** the system SHALL display a toast notification indicating the parent selection would create a circular reference

### Requirement: Parent department dropdown excludes self and descendants
The system SHALL exclude the current department and its descendants from the parent department dropdown when editing a department.

#### Scenario: Editing a department with children
- **WHEN** the user edits a department that has subdepartments
- **THEN** the parent dropdown SHALL NOT include the department itself or any of its descendant departments

#### Scenario: Creating a new department
- **WHEN** the user creates a new department
- **THEN** the parent dropdown SHALL list all existing departments

### Requirement: Delete a department with confirmation
The system SHALL allow deleting a department via the "Delete" action on a department row, with a confirmation dialog.

#### Scenario: Successful deletion
- **WHEN** the user clicks "Delete" on a department row and confirms the deletion
- **THEN** the system SHALL send a `DELETE /api/departments/{id}` request and refresh the departments list

#### Scenario: Deletion cancelled
- **WHEN** the user clicks "Delete" on a department row and cancels the confirmation
- **THEN** the system SHALL NOT send a delete request

#### Scenario: Department has assigned employees
- **WHEN** the user confirms deletion and the API returns `409 Conflict` indicating employees are assigned
- **THEN** the system SHALL display a toast notification indicating the department is in use and cannot be deleted

#### Scenario: Department has subdepartments
- **WHEN** the user confirms deletion and the API returns `409 Conflict` indicating subdepartments exist
- **THEN** the system SHALL display a toast notification indicating the department has subdepartments and cannot be deleted

### Requirement: Navigation entry for departments
The system SHALL include a "Departments" link in the application sidebar that navigates to `/departments`.

#### Scenario: Navigation to departments page
- **WHEN** the user clicks "Departments" in the sidebar
- **THEN** the system SHALL navigate to `/departments` and the sidebar link SHALL be visually marked as active
