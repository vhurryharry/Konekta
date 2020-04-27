using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WCA.Data.Migrations
{
    public partial class AddedGlobalXDocumentVersionState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null) throw new ArgumentNullException(nameof(migrationBuilder));

            migrationBuilder.AlterColumn<string>(
                name: "ProcessingStatus",
                schema: "WCA",
                table: "GlobalXTransactionStates",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastDocumentSyncUtc",
                schema: "WCA",
                table: "GlobalXOrgSettings",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "GlobalXDocumentVersionStates",
                schema: "WCA",
                columns: table => new
                {
                    DocumentVersionId = table.Column<Guid>(nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    LastUpdatedUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    DocumentId = table.Column<Guid>(nullable: false),
                    OrderId = table.Column<string>(nullable: true),
                    ActionstepOrgKey = table.Column<string>(nullable: true),
                    MatterId = table.Column<int>(nullable: false),
                    DocumentCopyStatus = table.Column<string>(nullable: false),
                    DocumentCopyStatusUpdatedUtc = table.Column<DateTime>(nullable: false),
                    LastError = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    StatusDescription = table.Column<string>(nullable: true),
                    TimestampUtc = table.Column<DateTime>(nullable: false),
                    OrderDateUtc = table.Column<DateTime>(nullable: false),
                    GlobalXUserId = table.Column<string>(nullable: true),
                    Criteria = table.Column<string>(nullable: true),
                    OrderType = table.Column<string>(nullable: true),
                    ItemNumber = table.Column<int>(nullable: true),
                    MimeType = table.Column<string>(nullable: true),
                    ActionstepSharePointUrl = table.Column<string>(nullable: true),
                    ActionstepActionDocumentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlobalXDocumentVersionStates", x => x.DocumentVersionId);
                    table.ForeignKey(
                        name: "FK_GlobalXDocumentVersionStates_ActionstepOrgs_ActionstepOrgKey",
                        column: x => x.ActionstepOrgKey,
                        principalSchema: "WCA",
                        principalTable: "ActionstepOrgs",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GlobalXDocumentVersionStates_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "WCA",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GlobalXDocumentVersionStates_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalSchema: "WCA",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GlobalXTransactionStates_ProcessingStatus",
                schema: "WCA",
                table: "GlobalXTransactionStates",
                column: "ProcessingStatus");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalXDocumentVersionStates_ActionstepOrgKey",
                schema: "WCA",
                table: "GlobalXDocumentVersionStates",
                column: "ActionstepOrgKey");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalXDocumentVersionStates_CreatedById",
                schema: "WCA",
                table: "GlobalXDocumentVersionStates",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalXDocumentVersionStates_DocumentCopyStatus",
                schema: "WCA",
                table: "GlobalXDocumentVersionStates",
                column: "DocumentCopyStatus");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalXDocumentVersionStates_DocumentId",
                schema: "WCA",
                table: "GlobalXDocumentVersionStates",
                column: "DocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalXDocumentVersionStates_MatterId",
                schema: "WCA",
                table: "GlobalXDocumentVersionStates",
                column: "MatterId");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalXDocumentVersionStates_OrderId",
                schema: "WCA",
                table: "GlobalXDocumentVersionStates",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalXDocumentVersionStates_UpdatedById",
                schema: "WCA",
                table: "GlobalXDocumentVersionStates",
                column: "UpdatedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null) throw new ArgumentNullException(nameof(migrationBuilder));

            migrationBuilder.DropTable(
                name: "GlobalXDocumentVersionStates",
                schema: "WCA");

            migrationBuilder.DropIndex(
                name: "IX_GlobalXTransactionStates_ProcessingStatus",
                schema: "WCA",
                table: "GlobalXTransactionStates");

            migrationBuilder.DropColumn(
                name: "LastDocumentSyncUtc",
                schema: "WCA",
                table: "GlobalXOrgSettings");

            migrationBuilder.AlterColumn<string>(
                name: "ProcessingStatus",
                schema: "WCA",
                table: "GlobalXTransactionStates",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
