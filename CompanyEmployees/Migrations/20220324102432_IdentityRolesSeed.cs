using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyEmployees.Migrations
{
    /// <inheritdoc />
    public partial class IdentityRolesSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "98ecbfbb-aa04-4629-8fa6-1210b1a20d76", "3ba23ee8-2406-41d3-99ad-61ee30b59447", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "edc19637-6bdb-45f1-95a5-dff49d5578e3", "d3be492d-0cdd-4913-a952-3d34a4b7bfd8", "Administrator", "ADMINISTRATOR" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "98ecbfbb-aa04-4629-8fa6-1210b1a20d76");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "edc19637-6bdb-45f1-95a5-dff49d5578e3");
        }
    }
}
