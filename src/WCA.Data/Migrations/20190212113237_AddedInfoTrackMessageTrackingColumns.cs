using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WCA.Data.Migrations
{
    public partial class AddedInfoTrackMessageTrackingColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.AddColumn<int>(
                name: "ProcessingStatus",
                schema: "WCA",
                table: "InfoTrackOrderUpdateMessageHistory",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ProcessingStatusUpdatedUtc",
                schema: "WCA",
                table: "InfoTrackOrderUpdateMessageHistory",
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
                name: "ProcessingStatus",
                schema: "WCA",
                table: "InfoTrackOrderUpdateMessageHistory");

            migrationBuilder.DropColumn(
                name: "ProcessingStatusUpdatedUtc",
                schema: "WCA",
                table: "InfoTrackOrderUpdateMessageHistory");
        }
    }
}
