---
name: file-structure-project
description: Define and scaffold the standard repository structure for projects that use a root solution file, source code under src, and dedicated infra, pipelines, and openspec folders.
---

Define or scaffold a project using the standard repository structure.

Use this skill when the user wants to:
- create a new project from scratch
- reorganize an existing repository to match the standard layout
- verify that a repository follows the expected folder structure

## Standard Structure

The repository should follow this layout:

```text
<repo-root>/
├── <solution-name>.sln
├── README.md
├── src/
├── infra/
├── pipelines/
└── openspec/
```

## Required Rules

1. The `.sln` file lives in the repository root.
2. Application and library projects live under `src/`.
3. Unit test projects live under `src/`.
4. Infrastructure-related assets live under `infra/`.
5. CI/CD definitions and automation files live under `pipelines/`.
6. OpenSpec files live under `openspec/`.
7. The repository root includes a `README.md`.

## Folder Intent

### `src/`

Use `src/` for all source projects.
Unit test projects also live under `src/`.

Examples:
- API projects
- worker services
- class libraries
- unit test projects

Do not place the solution file inside `src/`.

### `infra/`

Use `infra/` for deployment and environment assets.

Examples:
- IaC templates
- Bicep
- environment configuration templates

### `pipelines/`

Use `pipelines/` for build and release automation.

Examples:
- CI pipeline YAML files

### `openspec/`

Use `openspec/` for specification-driven change management artifacts.

Examples:
- `config.yaml`
- `changes/`
- `specs/`

### `src/*.Infrastructure` project internal structure

Infrastructure projects SHALL be organised by infrastructure concern, not by technical pattern.
Each concern gets its own top-level folder inside the project.

```text
<Name>.Infrastructure/
├── Database/
│   ├── AppDbContext.cs
│   ├── Configurations/     ← EF Core IEntityTypeConfiguration<T> files
│   └── Migrations/         ← EF Core migrations (--output-dir Database/Migrations)
├── ServiceBus/             ← future: Azure Service Bus clients, publishers, consumers
├── Storage/                ← future: blob storage, file storage
└── ...                     ← one folder per infrastructure concern
```

**Rules:**
- Group by concern (`Database/`, `ServiceBus/`, `Storage/`), never by pattern (`Persistence/`, `Migrations/`).
- Migrations MUST live inside `Database/Migrations/`, not at the project root. Always pass `--output-dir Database/Migrations` when running `dotnet ef migrations add`.
- Each concern folder is self-contained: its context, configuration, and data files all live together.
- Add placeholder folders for anticipated concerns (e.g. `ServiceBus/`) so the structure communicates intent from the start.

### `README.md`

The root `README.md` should explain:
- what the project does
- how to build and run it
- where key folders are
- how OpenSpec is used in the repository

## Recommended Workflow

When applying this structure:

1. Inspect the current repository layout.
2. Identify whether a root solution file already exists.
3. Create any missing top-level folders: `src`, `infra`, `pipelines`, `openspec`.
4. Ensure the `.sln` file is at the repository root.
5. Move code projects under `src/` if they are scattered elsewhere.
6. Move unit test projects under `src/` if they are located elsewhere.
7. Keep infrastructure assets under `infra/`.
8. Keep pipeline definitions under `pipelines/`.
9. Preserve or create the root `README.md`.
10. Verify the final structure matches the standard layout.

## Output Expectations

When using this skill, produce:
- the target folder structure
- any files or folders that need to be created or moved
- any exceptions or conflicts found in the current repository
- the final structure after changes

## Guardrails

- Keep the `.sln` file at the repository root.
- Do not invent extra top-level folders unless the user asks for them.
- Preserve existing repository conventions unless they conflict with this structure.
- Prefer minimal moves and minimal churn when reorganizing an existing project.
- If a current layout conflicts with build or deployment assumptions, call that out before changing it.
