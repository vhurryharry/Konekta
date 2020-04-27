using Microsoft.EntityFrameworkCore.Migrations;
using System;
using WCA.Domain.InfoTrack;

namespace WCA.Data.Migrations
{
    public partial class AddedEnumValueConverter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.AlterColumn<string>(
                name: "ActionstepDocumentUploadStatus",
                schema: "WCA",
                table: "InfoTrackOrders",
                nullable: false,
                oldClrType: typeof(int));
            MigrateEnumToString(migrationBuilder, typeof(ActionstepDocumentUploadStatus));

            migrationBuilder.AlterColumn<string>(
                name: "ActionstepDisbursementStatus",
                schema: "WCA",
                table: "InfoTrackOrders",
                nullable: false,
                oldClrType: typeof(int));
            MigrateEnumToString(migrationBuilder, typeof(ActionstepDisbursementStatus));

            migrationBuilder.AlterColumn<string>(
                name: "ActionstepDataCollectionUpdateStatus",
                schema: "WCA",
                table: "InfoTrackOrders",
                nullable: false,
                oldClrType: typeof(int));
        }

        private static void MigrateEnumToString(MigrationBuilder migrationBuilder, Type enumType)
        {
            foreach (var value in Enum.GetValues(enumType))
            {
                migrationBuilder.Sql(
                    $"UPDATE WCA.InfoTrackOrders " +
                    $"SET {enumType.Name}='{Enum.GetName(enumType, value)}' " +
                    $"WHERE {enumType.Name}='{(int)value}'");
            }
        }

        private static void MigrateEnumToInt(MigrationBuilder migrationBuilder, Type enumType)
        {
            foreach (var value in Enum.GetValues(enumType))
            {
                migrationBuilder.Sql(
                    $"UPDATE WCA.InfoTrackOrders " +
                    $"SET {enumType.Name}='{(int)value}' " +
                    $"WHERE {enumType.Name}='{Enum.GetName(enumType, value)}'");
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            MigrateEnumToInt(migrationBuilder, typeof(ActionstepDocumentUploadStatus));
            migrationBuilder.AlterColumn<int>(
                name: "ActionstepDocumentUploadStatus",
                schema: "WCA",
                table: "InfoTrackOrders",
                nullable: false,
                oldClrType: typeof(string));

            MigrateEnumToInt(migrationBuilder, typeof(ActionstepDisbursementStatus));
            migrationBuilder.AlterColumn<int>(
                name: "ActionstepDisbursementStatus",
                schema: "WCA",
                table: "InfoTrackOrders",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "ActionstepDataCollectionUpdateStatus",
                schema: "WCA",
                table: "InfoTrackOrders",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
