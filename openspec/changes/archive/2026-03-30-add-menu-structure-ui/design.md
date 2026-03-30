## Context

The app has a single `AppLayout.vue` component with a fixed left sidebar. Currently it contains two hardcoded `<RouterLink>` entries (Roles, Departments) with a flat list structure. As new top-level sections are added (Dashboard, Time Entries, Projects, Employees), the navigation needs grouping to separate everyday user workflows from administrative configuration.

The frontend uses DaisyUI's `menu` component classes on top of Tailwind CSS. Vue Router handles all navigation via `<RouterLink>` and `<RouterView>`.

## Goals / Non-Goals

**Goals:**
- Group Roles and Departments under a collapsible/expandable Admin section in the sidebar
- Add top-level links for Dashboard, Time Entries, Projects, and Employees
- Add stub page components for the four new routes so routing works end-to-end
- Change the default redirect from `/roles` to `/dashboard`

**Non-Goals:**
- Implementing actual content for the new stub pages (Dashboard, Time Entries, Projects, Employees)
- Role-based access control on menu items (e.g., hiding Admin for non-admin users)
- Persisting the open/closed state of the Admin submenu across page loads

## Decisions

### 1. DaisyUI nested `<details>` / `<summary>` for Admin submenu
Use DaisyUI's built-in collapsible menu pattern (`<li><details><summary>Admin</summary><ul>...</ul></details></li>`) rather than a custom toggle with JS state.

**Why:** The existing nav already uses DaisyUI `menu` classes. The native `<details>`/`<summary>` approach requires zero JS, stays open on page load by default, and is consistent with the existing DaisyUI theming. A Pinia-managed open/close state would be overkill for a simple nested menu.

**Alternative considered:** `v-show` toggle with a ref — rejected because it adds reactive state and a click handler for something the browser handles natively.

### 2. Stub pages as minimal single-file components
New pages (DashboardPage, TimeEntriesPage, ProjectsPage, EmployeesPage) are created as minimal `.vue` files with only a heading, following the same naming convention as `RolesPage.vue` and `DepartmentsPage.vue`.

**Why:** Routing must work before content exists. Stubs allow navigation to be fully functional while feature work on each section happens in separate changes.

### 3. All routes defined inline in `router.ts`
Routes for the new pages are added directly to the existing `router.ts` flat array, consistent with the current pattern.

**Why:** The router is small and simple. Introducing lazy-loading or route grouping would be premature for the current scale.

## Risks / Trade-offs

- **Admin submenu open by default:** The `<details>` element is open by default (via `open` attribute), which means the submenu is always expanded on first load. This is the desired UX until there are enough menu items to warrant collapsing. → No mitigation needed now; can add `open` toggle logic later.
- **Stub pages with no content:** New routes resolve but show empty/placeholder pages. → Acceptable — each section will be filled in a dedicated change.
