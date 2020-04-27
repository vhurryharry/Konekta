using Microsoft.EntityFrameworkCore.Migrations;

namespace WCA.Data.Migrations
{
    public partial class RenamedInfoTrackColumnsToUtc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new System.ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.RenameColumn(
                name: "InfoTrackDateOrdered",
                schema: "WCA",
                table: "InfoTrackOrderUpdateMessageHistory",
                newName: "InfoTrackDateOrderedUtc");

            migrationBuilder.RenameColumn(
                name: "InfoTrackDateCompleted",
                schema: "WCA",
                table: "InfoTrackOrderUpdateMessageHistory",
                newName: "InfoTrackDateCompletedUtc");

            migrationBuilder.RenameColumn(
                name: "InfoTrackDateOrdered",
                schema: "WCA",
                table: "InfoTrackOrders",
                newName: "InfoTrackDateOrderedUtc");

            migrationBuilder.RenameColumn(
                name: "InfoTrackDateCompleted",
                schema: "WCA",
                table: "InfoTrackOrders",
                newName: "InfoTrackDateCompletedUtc");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new System.ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.RenameColumn(
                name: "InfoTrackDateOrderedUtc",
                schema: "WCA",
                table: "InfoTrackOrderUpdateMessageHistory",
                newName: "InfoTrackDateOrdered");

            migrationBuilder.RenameColumn(
                name: "InfoTrackDateCompletedUtc",
                schema: "WCA",
                table: "InfoTrackOrderUpdateMessageHistory",
                newName: "InfoTrackDateCompleted");

            migrationBuilder.RenameColumn(
                name: "InfoTrackDateOrderedUtc",
                schema: "WCA",
                table: "InfoTrackOrders",
                newName: "InfoTrackDateOrdered");

            migrationBuilder.RenameColumn(
                name: "InfoTrackDateCompletedUtc",
                schema: "WCA",
                table: "InfoTrackOrders",
                newName: "InfoTrackDateCompleted");
        }
    }
}
