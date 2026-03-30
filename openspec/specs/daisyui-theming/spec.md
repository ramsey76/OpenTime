## Purpose

TBD

## Requirements

### Requirement: DaisyUI theme is applied
The application SHALL use DaisyUI with the `corporate` theme as the default visual theme for all UI components.

#### Scenario: Application loads with corporate theme
- **WHEN** the application is loaded in a browser
- **THEN** all UI components (buttons, tables, modals, form inputs, navigation) SHALL be rendered using DaisyUI's `corporate` theme styling

### Requirement: UI components use DaisyUI semantic classes
All interactive UI components SHALL use DaisyUI semantic class names instead of raw Tailwind utility classes for component-level styling.

#### Scenario: Buttons use DaisyUI classes
- **WHEN** a button is rendered (primary action, cancel, edit, delete)
- **THEN** it SHALL use DaisyUI `btn` classes (e.g., `btn btn-primary`, `btn btn-ghost`, `btn btn-error`)

#### Scenario: Tables use DaisyUI classes
- **WHEN** a data table is rendered (roles list, departments list)
- **THEN** it SHALL use the DaisyUI `table` class

#### Scenario: Modals use DaisyUI classes
- **WHEN** a modal is displayed (form modal, confirmation dialog)
- **THEN** it SHALL use DaisyUI `modal` and `modal-box` classes

#### Scenario: Form inputs use DaisyUI classes
- **WHEN** a form input, textarea, or select is rendered
- **THEN** it SHALL use DaisyUI `input`, `textarea`, or `select` classes with the `bordered` variant
