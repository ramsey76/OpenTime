## Why

The application needs a database schema to support its core functionality: tracking employee work hours against projects. Without a foundational schema, no feature development can begin. This establishes the essential data model for time tracking.

## What Changes

- Introduce an `Employees` table to store employee identity (authentication handled externally by Entra ID)
- Introduce a `Roles` table to define application-level authorization roles
- Introduce a `Projects` table to define trackable work items
- Introduce a `ProjectAssignments` table to assign employees to projects they can log time against
- Introduce a `TimeEntries` table to record hours logged by employees against their assigned projects
- Introduce a `Departments` table to record the department an employee belongs to
- Set up Entity Framework Core migrations for schema management
- Configure PostgreSQL as the database provider

## Capabilities

### New Capabilities
- `employee-management`: Core employee entity — identity fields, linked to Entra ID for authentication
- `authorization`: Role-based authorization — roles, employee-role assignments, and permissions
- `department-management`: Department entity — grouping employees by organizational unit
- `project-management`: Project entity — definition, status, and employee assignment
- `time-tracking`: Time entry entity — logging hours, linking employees to projects with timestamps

### Modified Capabilities
<!-- None — this is a greenfield schema -->

## Impact

- **Database**: New PostgreSQL schema with six core tables and their relationships
- **Backend**: EF Core DbContext, entity configurations, and initial migration
- **API**: None yet — this change covers the data layer only
- **Frontend**: None
