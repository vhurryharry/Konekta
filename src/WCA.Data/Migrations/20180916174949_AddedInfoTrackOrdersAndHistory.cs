using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WCA.Data.Migrations
{
    public partial class AddedInfoTrackOrdersAndHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.CreateTable(
                name: "InfoTrackOrders",
                schema: "WCA",
                columns: table => new
                {
                    ActionstepOrgKey = table.Column<string>(nullable: true),
                    ActionstepMatterId = table.Column<int>(nullable: false),
                    OrderedByWCAUserId = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    LastUpdated = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    ActionstepDisbursementStatus = table.Column<int>(nullable: false),
                    ActionstepDisbursementStatusUpdated = table.Column<DateTime>(nullable: false),
                    ActionstepDocumentUploadStatus = table.Column<int>(nullable: false),
                    ActionstepDocumentUploadStatusUpdated = table.Column<DateTime>(nullable: false),
                    ActionstepDataCollectionUpdateStatus = table.Column<int>(nullable: false),
                    ActionstepDataCollectionUpdateStatusUpdated = table.Column<DateTime>(nullable: false),
                    InfoTrackAvailableOnline = table.Column<bool>(nullable: false),
                    InfoTrackBillingTypeName = table.Column<string>(nullable: true),
                    InfoTrackClientReference = table.Column<string>(nullable: true),
                    InfoTrackDateOrdered = table.Column<DateTime>(nullable: false),
                    InfoTrackDateCompleted = table.Column<DateTime>(nullable: false),
                    InfoTrackDescription = table.Column<string>(nullable: true),
                    InfoTrackOrderId = table.Column<int>(nullable: false),
                    InfoTrackParentOrderId = table.Column<int>(nullable: false),
                    InfoTrackOrderedBy = table.Column<string>(nullable: true),
                    InfoTrackReference = table.Column<string>(nullable: true),
                    InfoTrackRetailerReference = table.Column<string>(nullable: true),
                    InfoTrackRetailerFee = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    InfoTrackRetailerFeeGST = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    InfoTrackRetailerFeeTotal = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    InfoTrackSupplierFee = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    InfoTrackSupplierFeeGST = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    InfoTrackSupplierFeeTotal = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    InfoTrackTotalFee = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    InfoTrackTotalFeeGST = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    InfoTrackTotalFeeTotal = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    InfoTrackServiceName = table.Column<string>(nullable: true),
                    InfoTrackStatus = table.Column<string>(nullable: true),
                    InfoTrackStatusMessage = table.Column<string>(nullable: true),
                    InfoTrackDownloadUrl = table.Column<string>(nullable: true),
                    InfoTrackOnlineUrl = table.Column<string>(nullable: true),
                    InfoTrackIsBillable = table.Column<bool>(nullable: false),
                    InfoTrackFileHash = table.Column<string>(nullable: true),
                    InfoTrackEmail = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfoTrackOrders", x => x.InfoTrackOrderId);
                    table.ForeignKey(
                        name: "FK_InfoTrackOrders_ActionstepOrgs_ActionstepOrgKey",
                        column: x => x.ActionstepOrgKey,
                        principalSchema: "WCA",
                        principalTable: "ActionstepOrgs",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InfoTrackOrders_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "WCA",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InfoTrackOrders_AspNetUsers_OrderedByWCAUserId",
                        column: x => x.OrderedByWCAUserId,
                        principalSchema: "WCA",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InfoTrackOrders_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalSchema: "WCA",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InfoTrackOrderUpdateMessageHistory",
                schema: "WCA",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    LastUpdated = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    InfoTrackAvailableOnline = table.Column<bool>(nullable: false),
                    InfoTrackBillingTypeName = table.Column<string>(nullable: true),
                    InfoTrackClientReference = table.Column<string>(nullable: true),
                    InfoTrackDateOrdered = table.Column<DateTime>(nullable: false),
                    InfoTrackDateCompleted = table.Column<DateTime>(nullable: false),
                    InfoTrackDescription = table.Column<string>(nullable: true),
                    InfoTrackOrderId = table.Column<int>(nullable: false),
                    InfoTrackParentOrderId = table.Column<int>(nullable: false),
                    InfoTrackOrderedBy = table.Column<string>(nullable: true),
                    InfoTrackReference = table.Column<string>(nullable: true),
                    InfoTrackRetailerReference = table.Column<string>(nullable: true),
                    InfoTrackRetailerFee = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    InfoTrackRetailerFeeGST = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    InfoTrackRetailerFeeTotal = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    InfoTrackSupplierFee = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    InfoTrackSupplierFeeGST = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    InfoTrackSupplierFeeTotal = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    InfoTrackTotalFee = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    InfoTrackTotalFeeGST = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    InfoTrackTotalFeeTotal = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    InfoTrackServiceName = table.Column<string>(nullable: true),
                    InfoTrackStatus = table.Column<string>(nullable: true),
                    InfoTrackStatusMessage = table.Column<string>(nullable: true),
                    InfoTrackDownloadUrl = table.Column<string>(nullable: true),
                    InfoTrackOnlineUrl = table.Column<string>(nullable: true),
                    InfoTrackIsBillable = table.Column<bool>(nullable: false),
                    InfoTrackFileHash = table.Column<string>(nullable: true),
                    InfoTrackEmail = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfoTrackOrderUpdateMessageHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InfoTrackOrderUpdateMessageHistory_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "WCA",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InfoTrackOrderUpdateMessageHistory_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalSchema: "WCA",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InfoTrackOrders_ActionstepOrgKey",
                schema: "WCA",
                table: "InfoTrackOrders",
                column: "ActionstepOrgKey");

            migrationBuilder.CreateIndex(
                name: "IX_InfoTrackOrders_CreatedById",
                schema: "WCA",
                table: "InfoTrackOrders",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_InfoTrackOrders_OrderedByWCAUserId",
                schema: "WCA",
                table: "InfoTrackOrders",
                column: "OrderedByWCAUserId");

            migrationBuilder.CreateIndex(
                name: "IX_InfoTrackOrders_UpdatedById",
                schema: "WCA",
                table: "InfoTrackOrders",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_InfoTrackOrderUpdateMessageHistory_CreatedById",
                schema: "WCA",
                table: "InfoTrackOrderUpdateMessageHistory",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_InfoTrackOrderUpdateMessageHistory_UpdatedById",
                schema: "WCA",
                table: "InfoTrackOrderUpdateMessageHistory",
                column: "UpdatedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.DropTable(
                name: "InfoTrackOrders",
                schema: "WCA");

            migrationBuilder.DropTable(
                name: "InfoTrackOrderUpdateMessageHistory",
                schema: "WCA");
        }
    }
}
