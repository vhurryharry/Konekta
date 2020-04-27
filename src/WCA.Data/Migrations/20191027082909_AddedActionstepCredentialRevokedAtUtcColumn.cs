using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WCA.Data.Migrations
{
    public partial class AddedActionstepCredentialRevokedAtUtcColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null) throw new ArgumentNullException(nameof(migrationBuilder));

            migrationBuilder.AddColumn<DateTime>(
                name: "RevokedAtUtc",
                schema: "WCA",
                table: "ActionstepCredentials",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null) throw new ArgumentNullException(nameof(migrationBuilder));

            migrationBuilder.DropColumn(
                name: "RevokedAtUtc",
                schema: "WCA",
                table: "ActionstepCredentials");
        }
    }
}
