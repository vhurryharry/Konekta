using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WCA.Data.Migrations
{
    public partial class AddedActionstepCredentialsLockSupport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null) throw new ArgumentNullException(nameof(migrationBuilder));

            migrationBuilder.AddColumn<int>(
                name: "ExpiresIn",
                schema: "WCA",
                table: "ActionstepCredentials",
                nullable: false,
                defaultValue: 0);

            // Set to 3600 only for existing records. New ones will have their value populated correctly from now on.
            migrationBuilder.Sql(@"UPDATE [WCA].[ActionstepCredentials] SET [ExpiresIn] = 3600");

            migrationBuilder.AddColumn<string>(
                name: "IdToken",
                schema: "WCA",
                table: "ActionstepCredentials",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LockExpiresAtUtc",
                schema: "WCA",
                table: "ActionstepCredentials",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "LockId",
                schema: "WCA",
                table: "ActionstepCredentials",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "ReceivedAtUtc",
                schema: "WCA",
                table: "ActionstepCredentials",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            // Calculate ReceivedAtUtc for existing values.
            migrationBuilder.Sql(@"UPDATE [WCA].[ActionstepCredentials]
                                    SET [ReceivedAtUtc] = DATEADD(second, -[ExpiresIn], [AccessTokenExpiryUtc]);");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null) throw new ArgumentNullException(nameof(migrationBuilder));

            migrationBuilder.DropColumn(
                name: "ExpiresIn",
                schema: "WCA",
                table: "ActionstepCredentials");

            migrationBuilder.DropColumn(
                name: "IdToken",
                schema: "WCA",
                table: "ActionstepCredentials");

            migrationBuilder.DropColumn(
                name: "LockExpiresAtUtc",
                schema: "WCA",
                table: "ActionstepCredentials");

            migrationBuilder.DropColumn(
                name: "LockId",
                schema: "WCA",
                table: "ActionstepCredentials");

            migrationBuilder.DropColumn(
                name: "ReceivedAtUtc",
                schema: "WCA",
                table: "ActionstepCredentials");
        }
    }
}
