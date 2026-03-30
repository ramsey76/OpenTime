## Why

The frontend currently uses raw Tailwind utility classes for all UI elements (buttons, tables, modals, forms, sidebar). This requires manually styling every component and produces an inconsistent visual feel. DaisyUI provides semantic component classes and pre-built themes, giving a polished, consistent look with minimal effort.

## What Changes

- Install DaisyUI as a Tailwind CSS plugin
- Apply a DaisyUI theme (e.g., `corporate`) to the application
- Refactor existing components to use DaisyUI semantic classes (`btn`, `table`, `modal`, `input`, `select`, `navbar`, `menu`, etc.)
- Replace custom toast and confirmation dialog styling with DaisyUI `alert` and `modal` patterns

## Capabilities

### New Capabilities
<!-- None — this is a styling refactor, not a new behavioural capability -->

### Modified Capabilities
<!-- No spec-level behaviour changes. DaisyUI replaces CSS classes but the functional requirements (CRUD flows, validation, error handling) remain identical. -->

## Impact

- **Frontend**: All Vue components in `src/TimeManagement.Web/src/` will have class attributes updated
- **Dependencies**: New npm dependency: `daisyui`
- **Backend**: No changes
- **Behaviour**: No functional changes — only visual appearance
