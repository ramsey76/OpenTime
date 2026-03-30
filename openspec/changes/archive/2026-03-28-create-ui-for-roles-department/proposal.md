## Why

The role and department APIs are implemented but there is no user interface for managing them. Administrators need a way to create, edit, and delete roles and departments through the application instead of relying on direct API calls.

## What Changes

- Add a Roles management page with a list view and create/edit/delete functionality
- Add a Departments management page with a hierarchical list view (supporting subdepartments) and create/edit/delete functionality
- Add navigation entries for both pages in the application sidebar/menu
- Integrate with the existing `/api/roles` and `/api/departments` endpoints

## Capabilities

### New Capabilities
- `roles-ui`: Vue.js pages and components for listing, creating, editing, and deleting roles
- `departments-ui`: Vue.js pages and components for listing (with hierarchy), creating, editing, and deleting departments — including parent department selection

### Modified Capabilities
<!-- None — the backend APIs already exist, this adds frontend only -->

## Impact

- **Frontend**: New Vue.js pages, components, Pinia stores, and route definitions for roles and departments
- **Backend**: No changes — consumes existing API endpoints
- **Dependencies**: No new dependencies expected (uses existing Vue.js, Pinia, Tailwind CSS stack)
