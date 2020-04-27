using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WCA.Data.Migrations
{
    public partial class RemoveInfoTrackOrdersDataCollection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.DropColumn(
                name: "ActionstepDataCollectionUpdateStatus",
                schema: "WCA",
                table: "InfoTrackOrders");

            migrationBuilder.DropColumn(
                name: "ActionstepDataCollectionUpdateStatusUpdated",
                schema: "WCA",
                table: "InfoTrackOrders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.AddColumn<string>(
                name: "ActionstepDataCollectionUpdateStatus",
                schema: "WCA",
                table: "InfoTrackOrders",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ActionstepDataCollectionUpdateStatusUpdated",
                schema: "WCA",
                table: "InfoTrackOrders",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
