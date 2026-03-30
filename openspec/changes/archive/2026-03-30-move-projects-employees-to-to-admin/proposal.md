## Why

Projects and Employees are administrative data that managers configure, not everyday tools that employees use — they belong alongside Roles and Departments in the Admin submenu rather than cluttering the top-level navigation. Moving them now keeps the top-level menu focused on the core time-tracking workflow (Dashboard, Time Entries).

## What Changes

- Move the **Projects** and **Employees** sidebar links from top-level into the existing Admin submenu
- The Admin submenu now contains: Roles, Departments, Projects, Employees
- Top-level nav is reduced to: Dashboard, Time Entries

## Capabilities

### New Capabilities

_(none)_

### Modified Capabilities

- `menu-structure-ui`: The requirement "Navigation sidebar displays top-level sections" changes — Projects and Employees are no longer top-level links. The requirement "Admin submenu groups Roles and Departments" changes — it now also includes Projects and Employees.

## Impact

- `src/TimeManagement.Web/src/components/AppLayout.vue` — move two `<RouterLink>` entries from top-level list into the Admin `<details>` submenu
