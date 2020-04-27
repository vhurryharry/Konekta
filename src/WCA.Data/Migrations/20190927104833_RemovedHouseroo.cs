using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WCA.Data.Migrations
{
    public partial class RemovedHouseroo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.DropTable(
                name: "HouserooCredentials",
                schema: "WCA");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.CreateTable(
                name: "HouserooCredentials",
                schema: "WCA",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AccessToken = table.Column<string>(nullable: false),
                    AccessTokenExpiryUtc = table.Column<DateTime>(nullable: false),
                    ActionstepOrgKey = table.Column<string>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    DateCreatedUtc = table.Column<DateTime>(nullable: false),
                    HouserooCompanyId = table.Column<int>(nullable: false),
                    HouserooCompanyName = table.Column<string>(nullable: false),
                    HouserooUserEmail = table.Column<string>(nullable: false),
                    HouserooUserName = table.Column<string>(nullable: false),
                    HouserooUserRole = table.Column<string>(nullable: false),
                    LastUpdatedUtc = table.Column<DateTime>(nullable: false),
                    OwnerId = table.Column<string>(nullable: false),
                    RefreshToken = table.Column<string>(nullable: true),
                    RefreshTokenExpiryUtc = table.Column<DateTime>(nullable: false),
                    TokenType = table.Column<string>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HouserooCredentials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HouserooCredentials_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "WCA",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HouserooCredentials_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalSchema: "WCA",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HouserooCredentials_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalSchema: "WCA",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HouserooCredentials_CreatedById",
                schema: "WCA",
                table: "HouserooCredentials",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_HouserooCredentials_OwnerId",
                schema: "WCA",
                table: "HouserooCredentials",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_HouserooCredentials_UpdatedById",
                schema: "WCA",
                table: "HouserooCredentials",
                column: "UpdatedById");
        }
    }
}
