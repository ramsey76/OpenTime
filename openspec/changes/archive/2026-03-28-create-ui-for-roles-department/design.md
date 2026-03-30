## Context

The backend APIs for roles (`/api/roles`) and departments (`/api/departments`) are implemented with full CRUD operations. There is currently no frontend application — this change introduces the Vue.js frontend project and the first two management pages. The design choices made here will set the patterns for all future UI work.

Existing backend DTOs:
- `RoleDto(Id, Name, Description)`
- `DepartmentDto(Id, Name, Code, ParentDepartmentId)`

## Goals / Non-Goals

**Goals:**
- Scaffold a Vue 3 + TypeScript + Pinia + Tailwind CSS frontend project
- Build roles and departments management pages with full CRUD
- Display department hierarchy (parent-child relationships)
- Establish reusable patterns for future pages (stores, API client, form/table components)

**Non-Goals:**
- Authentication/authorization UI (Entra ID integration is a separate concern)
- Employee management or time entry pages
- Responsive/mobile layout optimization
- Internationalization (i18n)

## Decisions

### 1. Frontend project location: `src/TimeManagement.Web/`

Place the Vue app under `src/TimeManagement.Web/` alongside the existing backend projects.

**Why:** Follows the existing `src/TimeManagement.*` naming convention. Keeps frontend and backend in the same repository for simpler development workflow.

**Alternatives considered:**
- Separate `frontend/` directory at repo root → rejected to stay consistent with the `src/` structure.

### 2. Vite as build tool with Vue 3 Composition API

Use Vite for dev server and bundling. All components use `<script setup lang="ts">` syntax.

**Why:** Vite is the standard Vue 3 build tool with fast HMR. Composition API with `<script setup>` is the recommended Vue 3 pattern — more concise and better TypeScript support than Options API.

### 3. API client using a thin fetch wrapper

Create a simple typed API client module (`api/`) with functions per resource, not a generated SDK.

```
api/
  client.ts       ← base fetch wrapper (base URL, error handling)
  roles.ts        ← getRoles(), getRole(id), createRole(), updateRole(), deleteRole()
  departments.ts  ← getDepartments(), getDepartment(id), createDepartment(), ...
```

**Why:** The API surface is small and stable. A thin wrapper gives full control and keeps bundle size minimal. Generated clients (e.g., openapi-generator) add complexity without enough benefit at this scale.

**Alternatives considered:**
- Axios → rejected; fetch is built-in and sufficient for this use case.
- OpenAPI generated client → rejected; overhead not justified for a handful of endpoints.

### 4. One Pinia store per resource

```
stores/
  useRolesStore.ts
  useDepartmentsStore.ts
```

Each store holds the list state, loading/error flags, and actions that call the API client. Components read from the store; mutations go through store actions only.

**Why:** Keeps state management predictable. One store per resource is simple to reason about and test.

### 5. Page layout: table list + modal forms

Both roles and departments pages follow the same pattern:
- A page with a data table listing all records
- An "Add" button that opens a modal form for creation
- Row-level "Edit" and "Delete" actions
- Edit opens the same modal form pre-filled
- Delete shows a confirmation dialog

```
┌─────────────────────────────────────────────────┐
│  Roles                              [+ Add Role]│
├─────────────────────────────────────────────────┤
│  Name          Description          Actions     │
│  ──────────    ──────────────       ─────────   │
│  Admin         Full access          Edit | Del  │
│  Viewer        Read-only access     Edit | Del  │
│  Manager       Team management      Edit | Del  │
└─────────────────────────────────────────────────┘
```

**Why:** Consistent UX pattern across both pages. Modals keep the user in context rather than navigating to separate form pages.

### 6. Department hierarchy: indented flat list with parent indicator

Display departments as a flat table but indent child departments and show their parent name. The create/edit form includes a dropdown to select a parent department (optional).

```
┌──────────────────────────────────────────────────────────┐
│  Departments                          [+ Add Department] │
├──────────────────────────────────────────────────────────┤
│  Name              Code    Parent         Actions        │
│  ──────────        ────    ──────         ─────────      │
│  Engineering       ENG     —              Edit | Del     │
│    └ Frontend      FE      Engineering    Edit | Del     │
│    └ Backend       BE      Engineering    Edit | Del     │
│  Operations        OPS     —              Edit | Del     │
└──────────────────────────────────────────────────────────┘
```

**Why:** Simpler than a full tree view component while still conveying hierarchy. The flat list with indentation works well for a shallow hierarchy. A collapsible tree can be introduced later if departments go deeper than 2-3 levels.

**Alternatives considered:**
- Full tree view component → rejected as over-engineering for the initial implementation.

### 7. Routing structure

```
/roles              → RolesPage.vue
/departments        → DepartmentsPage.vue
```

Use Vue Router with a simple layout component that includes a sidebar with navigation links.

### 8. Error handling: inline validation + toast notifications

- Form validation inline (required fields, max length) before submission
- API errors (409 Conflict, 422, etc.) shown as toast notifications
- Delete blocked (409 — role/department in use) shown as toast with the server message

**Why:** Inline validation gives immediate feedback. Toasts for server errors avoid blocking the UI.

## Risks / Trade-offs

- **First frontend project — no existing patterns to follow** → Mitigated by keeping the initial implementation simple and well-structured so it serves as the reference for future pages.
- **Flat indented list may not scale for deep hierarchies** → Acceptable for now; departments are unlikely to go beyond 3-4 levels. Can migrate to a tree component later.
- **No authentication in the UI yet** → The API is currently unauthenticated. Entra ID integration will be a separate change. For now, all users can access these pages.
