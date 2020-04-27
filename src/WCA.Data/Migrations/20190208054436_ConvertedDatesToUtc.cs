using Microsoft.EntityFrameworkCore.Migrations;

namespace WCA.Data.Migrations
{
    public partial class ConvertedDatesToUtc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new System.ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.RenameColumn(
                name: "LastUpdated",
                schema: "WCA",
                table: "InfoTrackOrderUpdateMessageHistory",
                newName: "LastUpdatedUtc");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                schema: "WCA",
                table: "InfoTrackOrderUpdateMessageHistory",
                newName: "DateCreatedUtc");

            migrationBuilder.RenameColumn(
                name: "LastUpdated",
                schema: "WCA",
                table: "InfoTrackOrders",
                newName: "LastUpdatedUtc");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                schema: "WCA",
                table: "InfoTrackOrders",
                newName: "DateCreatedUtc");

            migrationBuilder.RenameColumn(
                name: "ActionstepDocumentUploadStatusUpdated",
                schema: "WCA",
                table: "InfoTrackOrders",
                newName: "ActionstepDocumentUploadStatusUpdatedUtc");

            migrationBuilder.RenameColumn(
                name: "ActionstepDisbursementStatusUpdated",
                schema: "WCA",
                table: "InfoTrackOrders",
                newName: "ActionstepDisbursementStatusUpdatedUtc");

            migrationBuilder.RenameColumn(
                name: "LastUpdated",
                schema: "WCA",
                table: "IncompleteOrders",
                newName: "LastUpdatedUtc");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                schema: "WCA",
                table: "IncompleteOrders",
                newName: "DateCreatedUtc");

            migrationBuilder.RenameColumn(
                name: "LastUpdated",
                schema: "WCA",
                table: "HouserooCredentials",
                newName: "LastUpdatedUtc");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                schema: "WCA",
                table: "HouserooCredentials",
                newName: "DateCreatedUtc");

            migrationBuilder.RenameColumn(
                name: "LastUpdated",
                schema: "WCA",
                table: "ActionstepOrgs",
                newName: "LastUpdatedUtc");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                schema: "WCA",
                table: "ActionstepOrgs",
                newName: "DateCreatedUtc");

            migrationBuilder.RenameColumn(
                name: "LastUpdated",
                schema: "WCA",
                table: "ActionstepCredentials",
                newName: "LastUpdatedUtc");

            migrationBuilder.RenameColumn(
                name: "DateCreated",
                schema: "WCA",
                table: "ActionstepCredentials",
                newName: "DateCreatedUtc");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new System.ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.RenameColumn(
                name: "LastUpdatedUtc",
                schema: "WCA",
                table: "InfoTrackOrderUpdateMessageHistory",
                newName: "LastUpdated");

            migrationBuilder.RenameColumn(
                name: "DateCreatedUtc",
                schema: "WCA",
                table: "InfoTrackOrderUpdateMessageHistory",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedUtc",
                schema: "WCA",
                table: "InfoTrackOrders",
                newName: "LastUpdated");

            migrationBuilder.RenameColumn(
                name: "DateCreatedUtc",
                schema: "WCA",
                table: "InfoTrackOrders",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "ActionstepDocumentUploadStatusUpdatedUtc",
                schema: "WCA",
                table: "InfoTrackOrders",
                newName: "ActionstepDocumentUploadStatusUpdated");

            migrationBuilder.RenameColumn(
                name: "ActionstepDisbursementStatusUpdatedUtc",
                schema: "WCA",
                table: "InfoTrackOrders",
                newName: "ActionstepDisbursementStatusUpdated");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedUtc",
                schema: "WCA",
                table: "IncompleteOrders",
                newName: "LastUpdated");

            migrationBuilder.RenameColumn(
                name: "DateCreatedUtc",
                schema: "WCA",
                table: "IncompleteOrders",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedUtc",
                schema: "WCA",
                table: "HouserooCredentials",
                newName: "LastUpdated");

            migrationBuilder.RenameColumn(
                name: "DateCreatedUtc",
                schema: "WCA",
                table: "HouserooCredentials",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedUtc",
                schema: "WCA",
                table: "ActionstepOrgs",
                newName: "LastUpdated");

            migrationBuilder.RenameColumn(
                name: "DateCreatedUtc",
                schema: "WCA",
                table: "ActionstepOrgs",
                newName: "DateCreated");

            migrationBuilder.RenameColumn(
                name: "LastUpdatedUtc",
                schema: "WCA",
                table: "ActionstepCredentials",
                newName: "LastUpdated");

            migrationBuilder.RenameColumn(
                name: "DateCreatedUtc",
                schema: "WCA",
                table: "ActionstepCredentials",
                newName: "DateCreated");
        }
    }
}
