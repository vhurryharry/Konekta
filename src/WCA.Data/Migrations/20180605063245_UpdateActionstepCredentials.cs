using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WCA.Data.Migrations
{
    public partial class UpdateActionstepCredentials : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.RenameColumn(
                name: "NameIdentifier",
                schema: "WCA",
                table: "ActionstepCredentials",
                newName: "ApiEndpoint");

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiryUtc",
                schema: "WCA",
                table: "ActionstepCredentials",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiryUtc",
                schema: "WCA",
                table: "ActionstepCredentials");

            migrationBuilder.RenameColumn(
                name: "ApiEndpoint",
                schema: "WCA",
                table: "ActionstepCredentials",
                newName: "NameIdentifier");
        }
    }
}
