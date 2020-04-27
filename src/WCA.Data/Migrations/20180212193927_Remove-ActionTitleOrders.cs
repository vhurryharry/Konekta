using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WCA.Data.Migrations
{
    public partial class RemoveActionTitleOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.DropTable(
                name: "ActionTitleOrders",
                schema: "WCA");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.CreateTable(
                name: "ActionTitleOrders",
                schema: "WCA",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ActionstepOrgKey = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: false),
                    MatterId = table.Column<int>(nullable: false),
                    MetaData = table.Column<string>(type: "ntext", nullable: true),
                    TitleOrderId = table.Column<string>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionTitleOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionTitleOrders_ActionstepOrgs_ActionstepOrgKey",
                        column: x => x.ActionstepOrgKey,
                        principalSchema: "WCA",
                        principalTable: "ActionstepOrgs",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActionTitleOrders_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "WCA",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionTitleOrders_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalSchema: "WCA",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActionTitleOrders_ActionstepOrgKey",
                schema: "WCA",
                table: "ActionTitleOrders",
                column: "ActionstepOrgKey");

            migrationBuilder.CreateIndex(
                name: "IX_ActionTitleOrders_CreatedById",
                schema: "WCA",
                table: "ActionTitleOrders",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ActionTitleOrders_UpdatedById",
                schema: "WCA",
                table: "ActionTitleOrders",
                column: "UpdatedById");
        }
    }
}
