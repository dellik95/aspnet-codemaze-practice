using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyEmployees.Migrations
{
    /// <inheritdoc />
    public partial class RefreshTokenAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "98ecbfbb-aa04-4629-8fa6-1210b1a20d76");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "edc19637-6bdb-45f1-95a5-dff49d5578e3");

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokeExpireTime",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "81bd8bfb-295e-4b0f-a5b0-f88c8e24782c", "ca1b28dd-e3b2-45a8-9980-25b3dcafa0ff", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a65401e3-fde2-4eda-8602-1e682bdd60ef", "3edb8519-3845-4cdb-b4f0-da2e8666d2d1", "Manager", "MANAGER" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "81bd8bfb-295e-4b0f-a5b0-f88c8e24782c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a65401e3-fde2-4eda-8602-1e682bdd60ef");

            migrationBuilder.DropColumn(
                name: "RefreshTokeExpireTime",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "98ecbfbb-aa04-4629-8fa6-1210b1a20d76", "3ba23ee8-2406-41d3-99ad-61ee30b59447", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "edc19637-6bdb-45f1-95a5-dff49d5578e3", "d3be492d-0cdd-4913-a952-3d34a4b7bfd8", "Administrator", "ADMINISTRATOR" });
        }
    }
}
