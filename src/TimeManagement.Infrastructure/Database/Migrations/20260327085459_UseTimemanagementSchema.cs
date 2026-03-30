using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeManagement.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class UseTimemanagementSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "timemanagement");

            migrationBuilder.RenameTable(
                name: "TimeEntries",
                newName: "TimeEntries",
                newSchema: "timemanagement");

            migrationBuilder.RenameTable(
                name: "Roles",
                newName: "Roles",
                newSchema: "timemanagement");

            migrationBuilder.RenameTable(
                name: "Projects",
                newName: "Projects",
                newSchema: "timemanagement");

            migrationBuilder.RenameTable(
                name: "ProjectAssignments",
                newName: "ProjectAssignments",
                newSchema: "timemanagement");

            migrationBuilder.RenameTable(
                name: "Employees",
                newName: "Employees",
                newSchema: "timemanagement");

            migrationBuilder.RenameTable(
                name: "EmployeeRoles",
                newName: "EmployeeRoles",
                newSchema: "timemanagement");

            migrationBuilder.RenameTable(
                name: "Departments",
                newName: "Departments",
                newSchema: "timemanagement");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "TimeEntries",
                schema: "timemanagement",
                newName: "TimeEntries");

            migrationBuilder.RenameTable(
                name: "Roles",
                schema: "timemanagement",
                newName: "Roles");

            migrationBuilder.RenameTable(
                name: "Projects",
                schema: "timemanagement",
                newName: "Projects");

            migrationBuilder.RenameTable(
                name: "ProjectAssignments",
                schema: "timemanagement",
                newName: "ProjectAssignments");

            migrationBuilder.RenameTable(
                name: "Employees",
                schema: "timemanagement",
                newName: "Employees");

            migrationBuilder.RenameTable(
                name: "EmployeeRoles",
                schema: "timemanagement",
                newName: "EmployeeRoles");

            migrationBuilder.RenameTable(
                name: "Departments",
                schema: "timemanagement",
                newName: "Departments");
        }
    }
}
