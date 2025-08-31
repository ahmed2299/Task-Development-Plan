using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TDP.BLL.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "ID", "IDGuid", "NameAR", "NameEN" },
                values: new object[,]
                {
                    { 1, new Guid("f7a9e8b0-3c1d-4a9f-8b2a-1e6c9d7f8a3b"), "الموارد البشرية", "Human Resources" },
                    { 2, new Guid("a3b8d4c1-9e7f-4b0d-9c3a-2e8d7f6a5b4c"), "تقنية المعلومات", "Information Technology" },
                    { 3, new Guid("c9d8e7f6-5a4b-4c3d-8b2a-1e6d9c7f8a3b"), "المالية", "Finance" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "ID", "BirthDate", "DepartmentID", "Email", "HashedPassword", "IDGuid", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(1990, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "john.doe@example.com", "hashed_password_placeholder_1", new Guid("d1e2f3a4-b5c6-4d7e-8f9a-0b1c2d3e4f5a"), true, "John Doe" },
                    { 2, new DateTime(1988, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "jane.smith@example.com", "hashed_password_placeholder_2", new Guid("f5a4e3d2-c1b0-4a9f-8e7d-6c5b4a3f2e1d"), true, "Jane Smith" },
                    { 3, new DateTime(1992, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "peter.jones@example.com", "hashed_password_placeholder_3", new Guid("a9b8c7d6-e5f4-4a3b-8c2d-1e9f8a7b6c5d"), false, "Peter Jones" },
                    { 4, new DateTime(1995, 11, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, "sara.miller@example.com", "hashed_password_placeholder_4", new Guid("b3c2d1e0-f9a8-4e7d-6c5b-4a3f2e1d0c9b"), true, "Sara Miller" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "ID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "ID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "ID",
                keyValue: 3);
        }
    }
}
