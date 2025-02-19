using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ceylangiyim.Data.Migrations
{
    /// <inheritdoc />
    public partial class ColorAddedToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "UserGuid" },
                values: new object[] { new DateTime(2025, 2, 14, 19, 35, 48, 952, DateTimeKind.Local).AddTicks(977), new Guid("81baa5c2-89b8-4f06-914f-64b79f536c93") });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2025, 2, 14, 19, 35, 48, 952, DateTimeKind.Local).AddTicks(3989));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2025, 2, 14, 19, 35, 48, 952, DateTimeKind.Local).AddTicks(3998));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "UserGuid" },
                values: new object[] { new DateTime(2025, 2, 13, 17, 10, 59, 130, DateTimeKind.Local).AddTicks(7676), new Guid("b0425b54-f194-4a80-9fdd-a37f95d86678") });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2025, 2, 13, 17, 10, 59, 131, DateTimeKind.Local).AddTicks(597));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2025, 2, 13, 17, 10, 59, 131, DateTimeKind.Local).AddTicks(607));
        }
    }
}
