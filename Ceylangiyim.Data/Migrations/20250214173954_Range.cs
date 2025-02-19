using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ceylangiyim.Data.Migrations
{
    /// <inheritdoc />
    public partial class Range : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "UserGuid" },
                values: new object[] { new DateTime(2025, 2, 14, 20, 39, 53, 507, DateTimeKind.Local).AddTicks(9751), new Guid("aa1bf071-fdf1-4205-9344-c97f7bbd6647") });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2025, 2, 14, 20, 39, 53, 508, DateTimeKind.Local).AddTicks(2621));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2025, 2, 14, 20, 39, 53, 508, DateTimeKind.Local).AddTicks(2634));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreateDate", "UserGuid" },
                values: new object[] { new DateTime(2025, 2, 14, 20, 8, 36, 393, DateTimeKind.Local).AddTicks(3946), new Guid("779cb2e0-07f8-4f66-a113-1847d97a46e0") });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2025, 2, 14, 20, 8, 36, 393, DateTimeKind.Local).AddTicks(6608));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2025, 2, 14, 20, 8, 36, 393, DateTimeKind.Local).AddTicks(6617));
        }
    }
}
