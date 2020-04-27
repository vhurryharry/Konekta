using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WCA.Data.Migrations
{
    public partial class AddedGlobalXOrgSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null) throw new ArgumentNullException(nameof(migrationBuilder));

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ActionstepOrgs_Id",
                schema: "WCA",
                table: "ActionstepOrgs");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "WCA",
                table: "ActionstepOrgs");

            migrationBuilder.CreateTable(
                name: "GlobalXOrgSettings",
                schema: "WCA",
                columns: table => new
                {
                    ActionstepOrgKey = table.Column<string>(nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    LastUpdatedUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    AdminId = table.Column<string>(nullable: true),
                    TransactionSyncEnabled = table.Column<bool>(nullable: false),
                    DocumentSyncEnabled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlobalXOrgSettings", x => x.ActionstepOrgKey);
                    table.ForeignKey(
                        name: "FK_GlobalXOrgSettings_ActionstepOrgs_ActionstepOrgKey",
                        column: x => x.ActionstepOrgKey,
                        principalSchema: "WCA",
                        principalTable: "ActionstepOrgs",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GlobalXOrgSettings_AspNetUsers_AdminId",
                        column: x => x.AdminId,
                        principalSchema: "WCA",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GlobalXOrgSettings_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "WCA",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GlobalXOrgSettings_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalSchema: "WCA",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GlobalXOrgSettings_AdminId",
                schema: "WCA",
                table: "GlobalXOrgSettings",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalXOrgSettings_CreatedById",
                schema: "WCA",
                table: "GlobalXOrgSettings",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalXOrgSettings_UpdatedById",
                schema: "WCA",
                table: "GlobalXOrgSettings",
                column: "UpdatedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null) throw new ArgumentNullException(nameof(migrationBuilder));

            migrationBuilder.DropTable(
                name: "GlobalXOrgSettings",
                schema: "WCA");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "WCA",
                table: "ActionstepOrgs",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ActionstepOrgs_Id",
                schema: "WCA",
                table: "ActionstepOrgs",
                column: "Id");
        }
    }
}
