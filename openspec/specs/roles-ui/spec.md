## Purpose

TBD

## Requirements

### Requirement: Roles list page
The system SHALL display a page at `/roles` that lists all roles in a table with columns for Name, Description, and Actions.

#### Scenario: Roles exist
- **WHEN** the user navigates to `/roles`
- **THEN** the system SHALL fetch all roles from `GET /api/roles` and display them in a table showing name, description, and action buttons (Edit, Delete) per row

#### Scenario: No roles exist
- **WHEN** the user navigates to `/roles` and the API returns an empty array
- **THEN** the system SHALL display an empty state message indicating no roles have been created

#### Scenario: Loading state
- **WHEN** the user navigates to `/roles` and the API request is in progress
- **THEN** the system SHALL display a loading indicator

#### Scenario: API error on load
- **WHEN** the user navigates to `/roles` and the API request fails
- **THEN** the system SHALL display an error message

### Requirement: Create a role via modal form
The system SHALL provide a modal form to create a new role, triggered by an "Add Role" button on the roles page.

#### Scenario: Successful creation
- **WHEN** the user clicks "Add Role", fills in a valid name (and optional description), and submits
- **THEN** the system SHALL send a `POST /api/roles` request, close the modal, and refresh the roles list

#### Scenario: Name is required
- **WHEN** the user submits the create form without entering a name
- **THEN** the system SHALL display an inline validation error on the name field and SHALL NOT send an API request

#### Scenario: Name exceeds maximum length
- **WHEN** the user enters a name longer than 50 characters
- **THEN** the system SHALL display an inline validation error indicating the maximum length

#### Scenario: Duplicate name
- **WHEN** the user submits a name that already exists and the API returns `409 Conflict`
- **THEN** the system SHALL display a toast notification indicating the role name is already in use

#### Scenario: Cancel creation
- **WHEN** the user opens the create modal and clicks "Cancel" or closes the modal
- **THEN** the system SHALL close the modal without sending an API request

### Requirement: Edit a role via modal form
The system SHALL provide a modal form to edit an existing role, triggered by the "Edit" action on a role row.

#### Scenario: Modal opens pre-filled
- **WHEN** the user clicks "Edit" on a role row
- **THEN** the system SHALL open the role form modal pre-filled with the role's current name and description

#### Scenario: Successful update
- **WHEN** the user modifies the role fields and submits
- **THEN** the system SHALL send a `PUT /api/roles/{id}` request, close the modal, and refresh the roles list

#### Scenario: Duplicate name on update
- **WHEN** the user changes the name to one already used by another role and the API returns `409 Conflict`
- **THEN** the system SHALL display a toast notification indicating the role name is already in use

### Requirement: Delete a role with confirmation
The system SHALL allow deleting a role via the "Delete" action on a role row, with a confirmation dialog.

#### Scenario: Successful deletion
- **WHEN** the user clicks "Delete" on a role row and confirms the deletion
- **THEN** the system SHALL send a `DELETE /api/roles/{id}` request and refresh the roles list

#### Scenario: Deletion cancelled
- **WHEN** the user clicks "Delete" on a role row and cancels the confirmation
- **THEN** the system SHALL NOT send a delete request

#### Scenario: Role is assigned to employees
- **WHEN** the user confirms deletion and the API returns `409 Conflict`
- **THEN** the system SHALL display a toast notification indicating the role is in use and cannot be deleted

### Requirement: Navigation entry for roles
The system SHALL include a "Roles" link in the application sidebar that navigates to `/roles`.

#### Scenario: Navigation to roles page
- **WHEN** the user clicks "Roles" in the sidebar
- **THEN** the system SHALL navigate to `/roles` and the sidebar link SHALL be visually marked as active
