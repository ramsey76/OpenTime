## 1. Project Setup

- [x] 1.1 Install `Npgsql.EntityFrameworkCore.PostgreSQL` NuGet package
- [x] 1.2 Install `Microsoft.EntityFrameworkCore.Design` NuGet package (for migrations tooling)
- [x] 1.3 Add PostgreSQL connection string to `appsettings.json` and `appsettings.Development.json`

## 2. Entity Classes

- [x] 2.1 Create `Department` entity class with `Id`, `Name`, `Code` properties
- [x] 2.2 Create `Role` entity class with `Id`, `Name`, `Description` properties
- [x] 2.3 Create `Employee` entity class with `Id`, `ExternalId`, `FirstName`, `LastName`, `Email`, `DepartmentId` properties
- [x] 2.4 Create `EmployeeRole` join entity with `EmployeeId` and `RoleId` properties
- [x] 2.5 Create `Project` entity class with `Id`, `Name`, `Code`, `Status`, `Description` properties
- [x] 2.6 Create `ProjectAssignment` join entity with `EmployeeId` and `ProjectId` properties
- [x] 2.7 Create `TimeEntry` entity class with `Id`, `EmployeeId`, `ProjectId`, `Date`, `Hours`, `Description` properties

## 3. EF Core Configuration

- [x] 3.1 Create `AppDbContext` class inheriting from `DbContext` with `DbSet` properties for all entities
- [x] 3.2 Configure `Department`: unique index on `Code`, column length constraints
- [x] 3.3 Configure `Role`: unique index on `Name`, column length constraints
- [x] 3.4 Configure `Employee`: unique index on `ExternalId`, column lengths, FK to `Departments`
- [x] 3.5 Configure `EmployeeRole`: composite PK (`EmployeeId`, `RoleId`), FKs to `Employees` and `Roles`
- [x] 3.6 Configure `Project`: unique index on `Code`, column length constraints
- [x] 3.7 Configure `ProjectAssignment`: composite PK (`EmployeeId`, `ProjectId`), FKs to `Employees` and `Projects`
- [x] 3.8 Configure `TimeEntry`: column precision for `Hours` (decimal 4,2), FKs to `Employees` and `Projects`

## 4. DbContext Registration

- [x] 4.1 Register `AppDbContext` in `Program.cs` using the Npgsql provider and the configured connection string

## 5. Initial Migration

- [x] 5.1 Run `dotnet ef migrations add InitialSchema` to generate the initial migration
- [x] 5.2 Review the generated migration file to verify all tables, columns, constraints, and indexes are correct
- [x] 5.3 Run `dotnet ef database update` against the local PostgreSQL instance to apply the migration
- [x] 5.4 Verify all six tables exist in the database with correct columns and constraints
