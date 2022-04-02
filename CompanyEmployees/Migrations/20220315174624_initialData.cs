using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyEmployees.Migrations
{
    /// <inheritdoc />
    public partial class initialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "EmployeeId", "Address", "Country", "Name" },
                values: new object[] { 1, "583 Wall Dr. Gwynn Oak, MD 21207", "USA", "IT_Solutions Ltd" });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "EmployeeId", "Address", "Country", "Name" },
                values: new object[] { 2, "583 Wall Dr. Gwynn Oak, MD 21207", "UK", "Admin_Solutions Ltd" });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "EmployeeId", "Address", "Country", "Name" },
                values: new object[] { 3, "583 Wall Dr. Gwynn Oak, MD 21207", "UA", "User_Solutions Ltd" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "CompanyId", "Age", "CompanyId1", "Name", "Position" },
                values: new object[] { 1, 26, 1, "Alex", "Middle" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "CompanyId", "Age", "CompanyId1", "Name", "Position" },
                values: new object[] { 2, 21, 1, "Bill", "Junior" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "CompanyId", "Age", "CompanyId1", "Name", "Position" },
                values: new object[] { 3, 30, 2, "Ann", "QA" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "EmployeeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "CompanyId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "CompanyId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "CompanyId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "EmployeeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "EmployeeId",
                keyValue: 2);
        }
    }
}
