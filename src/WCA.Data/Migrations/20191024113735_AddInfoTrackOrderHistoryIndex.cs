using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace WCA.Data.Migrations
{
    public partial class AddInfoTrackOrderHistoryIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null) throw new ArgumentNullException(nameof(migrationBuilder));

            migrationBuilder.CreateIndex(
                name: "IX_InfoTrackOrderUpdateMessageHistory_InfoTrackOrderId",
                schema: "WCA",
                table: "InfoTrackOrderUpdateMessageHistory",
                column: "InfoTrackOrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null) throw new ArgumentNullException(nameof(migrationBuilder));

            migrationBuilder.DropIndex(
                name: "IX_InfoTrackOrderUpdateMessageHistory_InfoTrackOrderId",
                table: "InfoTrackOrderUpdateMessageHistory",
                schema: "WCA");
        }
    }
}
