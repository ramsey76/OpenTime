## Context

Following the `add-menu-structure-ui` change, the sidebar has four top-level links (Dashboard, Time Entries, Projects, Employees) and an Admin submenu (Roles, Departments). Projects and Employees are administrative concerns — they are configured by managers, not used daily by employees for time tracking. Moving them into Admin reduces top-level noise.

This change touches only `AppLayout.vue`. No routes, stores, or backend code are affected.

## Goals / Non-Goals

**Goals:**
- Move Projects and Employees `<RouterLink>` entries from the top-level `<ul>` into the Admin `<details>` submenu
- Leave all routes, page components, and other files untouched

**Non-Goals:**
- Reordering items within the Admin submenu
- Any visual or styling changes to the nav

## Decisions

### No structural changes needed
The existing DaisyUI `<details>`/`<summary>` Admin submenu already supports any number of child links. Adding Projects and Employees is a straightforward move of two `<li>` blocks — no new patterns or components required.

## Risks / Trade-offs

- **Users familiar with the old layout may be briefly confused** — Projects and Employees move from top-level to under Admin. Low risk given the app is early-stage.
