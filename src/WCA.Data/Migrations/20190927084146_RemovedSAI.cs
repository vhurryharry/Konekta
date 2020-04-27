using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WCA.Data.Migrations
{
    public partial class RemovedSAI : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.DropTable(
                name: "IncompleteOrders",
                schema: "WCA");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.CreateTable(
                name: "IncompleteOrders",
                schema: "WCA",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ActionstepOrgKey = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    DateCreatedUtc = table.Column<DateTime>(nullable: false),
                    LastUpdatedUtc = table.Column<DateTime>(nullable: false),
                    MatterId = table.Column<int>(nullable: false),
                    MatterType = table.Column<string>(nullable: false),
                    OwnerId = table.Column<string>(nullable: false),
                    PropertyIdentifier = table.Column<string>(nullable: false),
                    TitleOrderId = table.Column<string>(nullable: false),
                    TitleReference = table.Column<string>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncompleteOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncompleteOrders_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "WCA",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IncompleteOrders_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalSchema: "WCA",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IncompleteOrders_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalSchema: "WCA",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IncompleteOrders_CreatedById",
                schema: "WCA",
                table: "IncompleteOrders",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_IncompleteOrders_OwnerId",
                schema: "WCA",
                table: "IncompleteOrders",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_IncompleteOrders_UpdatedById",
                schema: "WCA",
                table: "IncompleteOrders",
                column: "UpdatedById");
        }
    }
}
