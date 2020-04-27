using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WCA.Data.Migrations
{
    public partial class AddedGlobalXMatterMappings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null) throw new ArgumentNullException(nameof(migrationBuilder));

            migrationBuilder.AlterColumn<string>(
                name: "MatterId",
                schema: "WCA",
                table: "GlobalXTransactionStates",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "MinimumMatterIdToSync",
                schema: "WCA",
                table: "GlobalXOrgSettings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "MatterId",
                schema: "WCA",
                table: "GlobalXDocumentVersionStates",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "GlobalXMatterMappings",
                schema: "WCA",
                columns: table => new
                {
                    ActionstepOrgKey = table.Column<string>(nullable: false),
                    GlobalXMatterId = table.Column<string>(nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    LastUpdatedUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    ActionstepMatterId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlobalXMatterMappings", x => new { x.ActionstepOrgKey, x.GlobalXMatterId });
                    table.ForeignKey(
                        name: "FK_GlobalXMatterMappings_ActionstepOrgs_ActionstepOrgKey",
                        column: x => x.ActionstepOrgKey,
                        principalSchema: "WCA",
                        principalTable: "ActionstepOrgs",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GlobalXMatterMappings_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "WCA",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GlobalXMatterMappings_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalSchema: "WCA",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GlobalXMatterMappings_CreatedById",
                schema: "WCA",
                table: "GlobalXMatterMappings",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalXMatterMappings_UpdatedById",
                schema: "WCA",
                table: "GlobalXMatterMappings",
                column: "UpdatedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null) throw new ArgumentNullException(nameof(migrationBuilder));

            migrationBuilder.DropTable(
                name: "GlobalXMatterMappings",
                schema: "WCA");

            migrationBuilder.DropColumn(
                name: "MinimumMatterIdToSync",
                schema: "WCA",
                table: "GlobalXOrgSettings");

            migrationBuilder.AlterColumn<int>(
                name: "MatterId",
                schema: "WCA",
                table: "GlobalXTransactionStates",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MatterId",
                schema: "WCA",
                table: "GlobalXDocumentVersionStates",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
