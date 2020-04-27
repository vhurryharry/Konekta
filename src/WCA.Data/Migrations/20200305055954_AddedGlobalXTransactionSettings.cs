using Microsoft.EntityFrameworkCore.Migrations;

namespace WCA.Data.Migrations
{
    public partial class AddedGlobalXTransactionSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null) throw new System.ArgumentNullException(nameof(migrationBuilder));

            migrationBuilder.DropForeignKey(
                name: "FK_GlobalXOrgSettings_AspNetUsers_AdminId",
                schema: "WCA",
                table: "GlobalXOrgSettings");

            migrationBuilder.DropIndex(
                name: "IX_GlobalXOrgSettings_AdminId",
                schema: "WCA",
                table: "GlobalXOrgSettings");

            migrationBuilder.DropColumn(
                name: "AdminId",
                schema: "WCA",
                table: "GlobalXOrgSettings");

            migrationBuilder.AddColumn<string>(
                name: "ActionstepSyncUserId",
                schema: "WCA",
                table: "GlobalXOrgSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GlobalXAdminId",
                schema: "WCA",
                table: "GlobalXOrgSettings",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LatestTransactionId",
                schema: "WCA",
                table: "GlobalXOrgSettings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TaxCodeIdNoGST",
                schema: "WCA",
                table: "GlobalXOrgSettings",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TaxCodeIdWithGST",
                schema: "WCA",
                table: "GlobalXOrgSettings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GlobalXOrgSettings_ActionstepSyncUserId",
                schema: "WCA",
                table: "GlobalXOrgSettings",
                column: "ActionstepSyncUserId");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalXOrgSettings_GlobalXAdminId",
                schema: "WCA",
                table: "GlobalXOrgSettings",
                column: "GlobalXAdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_GlobalXOrgSettings_AspNetUsers_ActionstepSyncUserId",
                schema: "WCA",
                table: "GlobalXOrgSettings",
                column: "ActionstepSyncUserId",
                principalSchema: "WCA",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GlobalXOrgSettings_AspNetUsers_GlobalXAdminId",
                schema: "WCA",
                table: "GlobalXOrgSettings",
                column: "GlobalXAdminId",
                principalSchema: "WCA",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null) throw new System.ArgumentNullException(nameof(migrationBuilder));

            migrationBuilder.DropForeignKey(
                name: "FK_GlobalXOrgSettings_AspNetUsers_ActionstepSyncUserId",
                schema: "WCA",
                table: "GlobalXOrgSettings");

            migrationBuilder.DropForeignKey(
                name: "FK_GlobalXOrgSettings_AspNetUsers_GlobalXAdminId",
                schema: "WCA",
                table: "GlobalXOrgSettings");

            migrationBuilder.DropIndex(
                name: "IX_GlobalXOrgSettings_ActionstepSyncUserId",
                schema: "WCA",
                table: "GlobalXOrgSettings");

            migrationBuilder.DropIndex(
                name: "IX_GlobalXOrgSettings_GlobalXAdminId",
                schema: "WCA",
                table: "GlobalXOrgSettings");

            migrationBuilder.DropColumn(
                name: "ActionstepSyncUserId",
                schema: "WCA",
                table: "GlobalXOrgSettings");

            migrationBuilder.DropColumn(
                name: "GlobalXAdminId",
                schema: "WCA",
                table: "GlobalXOrgSettings");

            migrationBuilder.DropColumn(
                name: "LatestTransactionId",
                schema: "WCA",
                table: "GlobalXOrgSettings");

            migrationBuilder.DropColumn(
                name: "TaxCodeIdNoGST",
                schema: "WCA",
                table: "GlobalXOrgSettings");

            migrationBuilder.DropColumn(
                name: "TaxCodeIdWithGST",
                schema: "WCA",
                table: "GlobalXOrgSettings");

            migrationBuilder.AddColumn<string>(
                name: "AdminId",
                schema: "WCA",
                table: "GlobalXOrgSettings",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GlobalXOrgSettings_AdminId",
                schema: "WCA",
                table: "GlobalXOrgSettings",
                column: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_GlobalXOrgSettings_AspNetUsers_AdminId",
                schema: "WCA",
                table: "GlobalXOrgSettings",
                column: "AdminId",
                principalSchema: "WCA",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
