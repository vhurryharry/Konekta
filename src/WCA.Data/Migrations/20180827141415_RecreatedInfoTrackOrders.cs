using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WCA.Data.Migrations
{
    public partial class RecreatedInfoTrackOrders : Migration
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
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    LastUpdated = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    ActionstepOrgKey = table.Column<string>(nullable: true),
                    ActionstepMatterId = table.Column<int>(nullable: false),
                    OrderedByWCAUserId = table.Column<string>(nullable: true),
                    AvailableOnline = table.Column<bool>(nullable: false),
                    BillingTypeName = table.Column<string>(nullable: true),
                    ClientReference = table.Column<string>(nullable: true),
                    DateOrdered = table.Column<DateTime>(nullable: false),
                    DateCompleted = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    OrderId = table.Column<int>(nullable: false),
                    ParentOrderId = table.Column<int>(nullable: false),
                    OrderedBy = table.Column<string>(nullable: true),
                    Reference = table.Column<string>(nullable: true),
                    RetailerReference = table.Column<string>(nullable: true),
                    RetailerFee = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    RetailerFeeGST = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    RetailerFeeTotal = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    SupplierFee = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    SupplierFeeGST = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    SupplierFeeTotal = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    TotalFee = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    TotalFeeGST = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    TotalFeeTotal = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    ServiceName = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    StatusMessage = table.Column<string>(nullable: true),
                    DownloadUrl = table.Column<string>(nullable: true),
                    OnlineUrl = table.Column<string>(nullable: true),
                    IsBillable = table.Column<bool>(nullable: false),
                    FileHash = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfoTrackOrders", x => x.Id);
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
        }
    }
}
