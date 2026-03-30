# Spec: menu-structure-ui

## Purpose

Defines the structure and behaviour of the application's navigation sidebar, including top-level sections, the Admin submenu, default routing, and stub pages for each navigable section.

## Requirements

### Requirement: Navigation sidebar displays top-level sections
The sidebar SHALL display top-level navigation links for Dashboard and Time Entries.

#### Scenario: All top-level links are visible
- **WHEN** the user opens the application
- **THEN** the sidebar displays links to Dashboard and Time Entries

#### Scenario: Active link is highlighted
- **WHEN** the user is on a page (e.g., `/dashboard`)
- **THEN** the corresponding sidebar link is visually highlighted as active

### Requirement: Admin submenu groups Roles and Departments
The sidebar SHALL contain an Admin section that groups the Roles, Departments, Projects, and Employees links.

#### Scenario: Admin submenu is expanded by default
- **WHEN** the user opens the application
- **THEN** the Admin section is visible and expanded, showing Roles, Departments, Projects, and Employees links

#### Scenario: Roles link navigates to the roles page
- **WHEN** the user clicks the Roles link inside the Admin section
- **THEN** the user is navigated to `/roles`

#### Scenario: Departments link navigates to the departments page
- **WHEN** the user clicks the Departments link inside the Admin section
- **THEN** the user is navigated to `/departments`

#### Scenario: Projects link navigates to the projects page
- **WHEN** the user clicks the Projects link inside the Admin section
- **THEN** the user is navigated to `/projects`

#### Scenario: Employees link navigates to the employees page
- **WHEN** the user clicks the Employees link inside the Admin section
- **THEN** the user is navigated to `/employees`

### Requirement: Default route redirects to Dashboard
The application SHALL redirect the root path (`/`) to `/dashboard`.

#### Scenario: Root path redirects
- **WHEN** the user navigates to `/`
- **THEN** the browser is redirected to `/dashboard`

### Requirement: Stub pages exist for new top-level sections
The application SHALL provide navigable pages for Dashboard, Time Entries, Projects, and Employees.

#### Scenario: Dashboard page loads
- **WHEN** the user navigates to `/dashboard`
- **THEN** a Dashboard page is rendered within the app layout

#### Scenario: Time Entries page loads
- **WHEN** the user navigates to `/time-entries`
- **THEN** a Time Entries page is rendered within the app layout

#### Scenario: Projects page loads
- **WHEN** the user navigates to `/projects`
- **THEN** a Projects page is rendered within the app layout

#### Scenario: Employees page loads
- **WHEN** the user navigates to `/employees`
- **THEN** an Employees page is rendered within the app layout
