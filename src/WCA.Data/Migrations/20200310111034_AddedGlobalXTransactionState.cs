using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WCA.Data.Migrations
{
    public partial class AddedGlobalXTransactionState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null) throw new ArgumentNullException(nameof(migrationBuilder));

            migrationBuilder.CreateTable(
                name: "GlobalXTransactionStates",
                schema: "WCA",
                columns: table => new
                {
                    TransactionId = table.Column<int>(nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    LastUpdatedUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    OrderId = table.Column<string>(nullable: true),
                    ActionstepOrgKey = table.Column<string>(nullable: true),
                    MatterId = table.Column<int>(nullable: false),
                    ProcessingStatus = table.Column<string>(nullable: false),
                    ProcessingStatusUpdatedUtc = table.Column<DateTime>(nullable: false),
                    LastError = table.Column<string>(nullable: true),
                    GSTTaxableDisbursementId = table.Column<int>(nullable: true),
                    GSTFreeDisbursementId = table.Column<int>(nullable: true),
                    TransactionDateTimeUtc = table.Column<DateTime>(nullable: false),
                    SearchReference = table.Column<string>(nullable: true),
                    WholesalePrice = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    WholesaleGst = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    RetailPrice = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    RetailGst = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    CreditForTransactionId = table.Column<int>(nullable: false),
                    ItemNumber = table.Column<int>(nullable: false),
                    MatterBasedInvoiced = table.Column<bool>(nullable: false),
                    GlobalXUserId = table.Column<string>(nullable: true),
                    GlobalXCustomerRef = table.Column<string>(nullable: true),
                    ProductCode = table.Column<string>(nullable: true),
                    ProductDescription = table.Column<string>(nullable: true),
                    ProductSubGroup = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlobalXTransactionStates", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_GlobalXTransactionStates_ActionstepOrgs_ActionstepOrgKey",
                        column: x => x.ActionstepOrgKey,
                        principalSchema: "WCA",
                        principalTable: "ActionstepOrgs",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GlobalXTransactionStates_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "WCA",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GlobalXTransactionStates_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalSchema: "WCA",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GlobalXTransactionStates_ActionstepOrgKey",
                schema: "WCA",
                table: "GlobalXTransactionStates",
                column: "ActionstepOrgKey");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalXTransactionStates_CreatedById",
                schema: "WCA",
                table: "GlobalXTransactionStates",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalXTransactionStates_MatterId",
                schema: "WCA",
                table: "GlobalXTransactionStates",
                column: "MatterId");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalXTransactionStates_OrderId",
                schema: "WCA",
                table: "GlobalXTransactionStates",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_GlobalXTransactionStates_UpdatedById",
                schema: "WCA",
                table: "GlobalXTransactionStates",
                column: "UpdatedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null) throw new ArgumentNullException(nameof(migrationBuilder));

            migrationBuilder.DropTable(
                name: "GlobalXTransactionStates",
                schema: "WCA");
        }
    }
}
