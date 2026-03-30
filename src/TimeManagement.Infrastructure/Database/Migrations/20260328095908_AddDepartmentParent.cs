using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TimeManagement.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddDepartmentParent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParentDepartmentId",
                schema: "timemanagement",
                table: "Departments",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_ParentDepartmentId",
                schema: "timemanagement",
                table: "Departments",
                column: "ParentDepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Departments_ParentDepartmentId",
                schema: "timemanagement",
                table: "Departments",
                column: "ParentDepartmentId",
                principalSchema: "timemanagement",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Departments_ParentDepartmentId",
                schema: "timemanagement",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_ParentDepartmentId",
                schema: "timemanagement",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "ParentDepartmentId",
                schema: "timemanagement",
                table: "Departments");
        }
    }
}
