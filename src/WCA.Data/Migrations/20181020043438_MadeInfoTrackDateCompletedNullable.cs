using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WCA.Data.Migrations
{
    public partial class MadeInfoTrackDateCompletedNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.AlterColumn<DateTime>(
                name: "InfoTrackDateCompleted",
                schema: "WCA",
                table: "InfoTrackOrderUpdateMessageHistory",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AlterColumn<DateTime>(
                name: "InfoTrackDateCompleted",
                schema: "WCA",
                table: "InfoTrackOrders",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.Sql(@"UPDATE [WCA].[InfoTrackOrders]
                                    SET [InfoTrackDateCompleted] = NULL
                                    WHERE [InfoTrackDateCompleted] = '0001-01-01 00:00:00.0000000'");

            migrationBuilder.Sql(@"UPDATE [WCA].[InfoTrackOrderUpdateMessageHistory]
                                    SET [InfoTrackDateCompleted] = NULL
                                    WHERE [InfoTrackDateCompleted] = '0001-01-01 00:00:00.0000000'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.Sql(@"UPDATE [WCA].[InfoTrackOrders]
                                    SET [InfoTrackDateCompleted] = '0001-01-01 00:00:00.0000000'
                                    WHERE [InfoTrackDateCompleted] is NULL");

            migrationBuilder.Sql(@"UPDATE [WCA].[InfoTrackOrderUpdateMessageHistory]
                                    SET [InfoTrackDateCompleted] = '0001-01-01 00:00:00.0000000'
                                    WHERE [InfoTrackDateCompleted] is NULL");

            migrationBuilder.AlterColumn<DateTime>(
                name: "InfoTrackDateCompleted",
                schema: "WCA",
                table: "InfoTrackOrderUpdateMessageHistory",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "InfoTrackDateCompleted",
                schema: "WCA",
                table: "InfoTrackOrders",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);
        }
    }
}
