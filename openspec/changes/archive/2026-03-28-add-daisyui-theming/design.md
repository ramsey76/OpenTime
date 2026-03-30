## Context

The frontend uses raw Tailwind utility classes for all visual styling. Every button, table, modal, and form input is styled manually with long class strings (e.g., `px-4 py-2 text-sm text-white bg-blue-600 rounded-md hover:bg-blue-700`). This works but is verbose and produces a generic look. DaisyUI is a Tailwind CSS component library that adds semantic class names (`btn`, `btn-primary`, `table`, `modal`, `input`, etc.) and ships with 30+ pre-built themes.

Current components that need updating:
- `AppLayout.vue` — sidebar navigation
- `RolesPage.vue` — table, buttons
- `DepartmentsPage.vue` — table, buttons
- `RoleFormModal.vue` — modal, form inputs, buttons
- `DepartmentFormModal.vue` — modal, form inputs, select, buttons
- `ToastNotification.vue` — alert/notification
- `ConfirmDialog.vue` — modal, buttons

## Goals / Non-Goals

**Goals:**
- Install DaisyUI and configure a theme
- Replace raw Tailwind utility classes with DaisyUI semantic classes across all components
- Achieve a consistent, polished visual design

**Non-Goals:**
- Changing any functional behaviour (API calls, validation, routing)
- Adding dark mode toggle or theme switching UI
- Replacing the sidebar layout structure

## Decisions

### 1. Theme: `corporate`

Use the `corporate` DaisyUI theme as the default.

**Why:** It's a clean, professional theme suited for business/admin applications. Neutral colors, clear contrast, no playful styling. Fits a time management tool's audience.

**Alternatives considered:**
- `business` → too dark for a primary admin interface
- `nord` → muted palette may reduce contrast for data-heavy tables
- `light` → too generic, doesn't differentiate from raw Tailwind

### 2. DaisyUI v5 with Tailwind v4 CSS-based config

Install DaisyUI v5 which is compatible with Tailwind v4. Configuration is done in `style.css` via `@plugin` and `@theme` directives — no `tailwind.config.js` needed.

```css
@import "tailwindcss";
@plugin "daisyui" {
  themes: corporate;
}
```

**Why:** Matches the existing Tailwind v4 CSS-based configuration approach. No additional config files.

### 3. Class mapping strategy

Replace utility class patterns with DaisyUI equivalents:

| Current Pattern | DaisyUI Replacement |
|---|---|
| `px-4 py-2 text-sm text-white bg-blue-600 rounded-md hover:bg-blue-700` | `btn btn-primary btn-sm` |
| `px-4 py-2 text-sm text-gray-700 bg-gray-100 rounded-md hover:bg-gray-200` | `btn btn-ghost btn-sm` |
| `text-red-600 hover:text-red-800` | `btn btn-link btn-error btn-sm` |
| `text-blue-600 hover:text-blue-800` | `btn btn-link btn-info btn-sm` |
| `<table>` with header/row classes | `table table-zebra` |
| Custom modal backdrop + card | `dialog` + `modal-box` |
| Custom toast with Transition | `toast` + `alert alert-error` |
| `<input>` with border/focus classes | `input input-bordered` |
| `<textarea>` with border/focus classes | `textarea textarea-bordered` |
| `<select>` with border/focus classes | `select select-bordered` |
| Sidebar nav links | `menu` with `menu-title` and active class |

### 4. Keep Tailwind utilities for layout

DaisyUI handles component styling; Tailwind utilities remain for layout (`flex`, `gap`, `p-6`, `w-full`, etc.). This is the intended usage — they're complementary, not replacements.

## Risks / Trade-offs

- **DaisyUI adds bundle size (~few KB)** → Acceptable; it replaces verbose utility strings so net file size change is minimal.
- **Theme might not match exact desired aesthetic** → Mitigated by choosing `corporate` which is neutral. Individual component colors can be overridden with Tailwind utilities if needed.
- **DaisyUI version compatibility with Tailwind v4** → DaisyUI v5 is built for Tailwind v4. Verified compatible with `@tailwindcss/vite` plugin approach.
