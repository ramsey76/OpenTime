## 1. Project Scaffold

- [x] 1.1 Scaffold Vue 3 + TypeScript project with Vite in `src/TimeManagement.Web/`
- [x] 1.2 Install and configure Pinia, Vue Router, and Tailwind CSS
- [x] 1.3 Create the base app layout component with a sidebar containing "Roles" and "Departments" navigation links
- [x] 1.4 Configure Vue Router with routes: `/roles` → RolesPage, `/departments` → DepartmentsPage

## 2. API Client

- [x] 2.1 Create `api/client.ts` — base fetch wrapper with configurable base URL and error handling
- [x] 2.2 Create `api/roles.ts` — typed functions: `getRoles()`, `getRole(id)`, `createRole(req)`, `updateRole(id, req)`, `deleteRole(id)`
- [x] 2.3 Create `api/departments.ts` — typed functions: `getDepartments()`, `getDepartment(id)`, `createDepartment(req)`, `updateDepartment(id, req)`, `deleteDepartment(id)`
- [x] 2.4 Define TypeScript types matching backend DTOs: `RoleDto`, `DepartmentDto`, `CreateRoleRequest`, `UpdateRoleRequest`, `CreateDepartmentRequest`, `UpdateDepartmentRequest`

## 3. Pinia Stores

- [x] 3.1 Create `stores/useRolesStore.ts` with state (roles list, loading, error), and actions (fetchRoles, createRole, updateRole, deleteRole)
- [x] 3.2 Create `stores/useDepartmentsStore.ts` with state (departments list, loading, error), and actions (fetchDepartments, createDepartment, updateDepartment, deleteDepartment)
- [x] 3.3 Add a computed getter in `useDepartmentsStore` that builds a hierarchical tree from the flat departments list for display

## 4. Shared UI Components

- [x] 4.1 Create a reusable toast notification component for displaying API error messages
- [x] 4.2 Create a reusable confirmation dialog component for delete actions

## 5. Roles UI

- [x] 5.1 Create `RolesPage.vue` with a table listing roles (Name, Description, Actions columns), loading state, empty state, and "Add Role" button
- [x] 5.2 Create `RoleFormModal.vue` — modal form with name (required, max 50) and description fields, inline validation, used for both create and edit
- [x] 5.3 Wire up create flow: "Add Role" → open modal → submit → POST API → close modal → refresh list
- [x] 5.4 Wire up edit flow: "Edit" action → open modal pre-filled → submit → PUT API → close modal → refresh list
- [x] 5.5 Wire up delete flow: "Delete" action → confirmation dialog → DELETE API → refresh list; show toast on 409 (role in use)

## 6. Departments UI

- [x] 6.1 Create `DepartmentsPage.vue` with a hierarchical table listing departments (Name with indentation, Code, Parent, Actions columns), loading state, empty state, and "Add Department" button
- [x] 6.2 Create `DepartmentFormModal.vue` — modal form with name (required, max 100), code (required, max 10), and optional parent department dropdown, inline validation
- [x] 6.3 Implement parent dropdown filtering: exclude current department and its descendants when editing; show all departments when creating
- [x] 6.4 Wire up create flow: "Add Department" → open modal → submit → POST API → close modal → refresh list
- [x] 6.5 Wire up edit flow: "Edit" action → open modal pre-filled (including parent selection) → submit → PUT API → close modal → refresh list
- [x] 6.6 Wire up delete flow: "Delete" action → confirmation dialog → DELETE API → refresh list; show toast on 409 (employees assigned or subdepartments exist)
- [x] 6.7 Handle 422 responses on create/edit (invalid parent) with a toast notification
