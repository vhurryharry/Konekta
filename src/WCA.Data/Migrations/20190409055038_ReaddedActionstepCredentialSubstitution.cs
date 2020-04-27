using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WCA.Data.Migrations
{
    public partial class ReaddedActionstepCredentialSubstitution : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.CreateTable(
                name: "ActionstepCredentialSubstitutions",
                schema: "WCA",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreatedUtc = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    LastUpdatedUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    ForOwnerId = table.Column<string>(nullable: true),
                    SubstituteWithOwnerId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionstepCredentialSubstitutions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ActionstepCredentialSubstitutions_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "WCA",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionstepCredentialSubstitutions_AspNetUsers_ForOwnerId",
                        column: x => x.ForOwnerId,
                        principalSchema: "WCA",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionstepCredentialSubstitutions_AspNetUsers_SubstituteWithOwnerId",
                        column: x => x.SubstituteWithOwnerId,
                        principalSchema: "WCA",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ActionstepCredentialSubstitutions_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalSchema: "WCA",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActionstepCredentialSubstitutions_CreatedById",
                schema: "WCA",
                table: "ActionstepCredentialSubstitutions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ActionstepCredentialSubstitutions_ForOwnerId",
                schema: "WCA",
                table: "ActionstepCredentialSubstitutions",
                column: "ForOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionstepCredentialSubstitutions_SubstituteWithOwnerId",
                schema: "WCA",
                table: "ActionstepCredentialSubstitutions",
                column: "SubstituteWithOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ActionstepCredentialSubstitutions_UpdatedById",
                schema: "WCA",
                table: "ActionstepCredentialSubstitutions",
                column: "UpdatedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.DropTable(
                name: "ActionstepCredentialSubstitutions",
                schema: "WCA");
        }
    }
}
