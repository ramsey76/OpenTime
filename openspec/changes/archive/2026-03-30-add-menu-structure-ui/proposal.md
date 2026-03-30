## Why

The app currently only has two menu items (Roles and Departments) with no top-level structure, and the default route redirects to `/roles`. As the app grows to include Employees, Projects, Time Entries, and a Dashboard, the navigation needs a clear hierarchy so users can distinguish between everyday workflows and administrative configuration.

## What Changes

- Introduce an **Admin** collapsible section in the sidebar that groups existing administrative links (Roles, Departments)
- Add top-level navigation links for: Dashboard, Time Entries, Projects, and Employees
- Add stub/placeholder page components for the new top-level sections
- Update Vue Router to add the new routes and change the default redirect to `/dashboard`
- Update `AppLayout.vue` to reflect the new menu structure

## Capabilities

### New Capabilities

- `menu-structure-ui`: Navigation sidebar structure with an Admin submenu containing Roles and Departments, plus top-level links for Dashboard, Time Entries, Projects, and Employees

### Modified Capabilities

- `roles-ui`: No requirement changes — Roles page remains unchanged, only its menu placement moves under Admin
- `departments-ui`: No requirement changes — Departments page remains unchanged, only its menu placement moves under Admin

## Impact

- `src/TimeManagement.Web/src/components/AppLayout.vue` — menu restructure
- `src/TimeManagement.Web/src/router.ts` — new routes, updated default redirect
- `src/TimeManagement.Web/src/pages/` — new stub page components (DashboardPage, TimeEntriesPage, ProjectsPage, EmployeesPage)
