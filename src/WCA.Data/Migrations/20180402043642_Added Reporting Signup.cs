using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WCA.Data.Migrations
{
    public partial class AddedReportingSignup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.CreateTable(
                name: "ReportSyncSignupSubmissions",
                schema: "WCA",
                columns: table => new
                {
                    ReportSyncSignupSubmissionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ABN = table.Column<string>(nullable: true),
                    AcceptedTermsAndConditions = table.Column<bool>(nullable: false),
                    AcknowledgedFeesAndCharges = table.Column<bool>(nullable: false),
                    ActionstepOrgKey = table.Column<string>(nullable: true),
                    Address1 = table.Column<string>(nullable: true),
                    Address2 = table.Column<string>(nullable: true),
                    BillingContactEmail = table.Column<string>(nullable: true),
                    BillingContactFirstname = table.Column<string>(nullable: true),
                    BillingContactLastname = table.Column<string>(nullable: true),
                    BillingFrequency = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Company = table.Column<string>(nullable: true),
                    CreatedById = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Postcode = table.Column<string>(nullable: true),
                    ServiceContactEmail = table.Column<string>(nullable: true),
                    ServiceContactFirstname = table.Column<string>(nullable: true),
                    ServiceContactLastname = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    StripeId = table.Column<string>(nullable: true),
                    SubmittedDateTimeUtc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportSyncSignupSubmissions", x => x.ReportSyncSignupSubmissionId);
                    table.ForeignKey(
                        name: "FK_ReportSyncSignupSubmissions_ActionstepOrgs_ActionstepOrgKey",
                        column: x => x.ActionstepOrgKey,
                        principalSchema: "WCA",
                        principalTable: "ActionstepOrgs",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReportSyncSignupSubmissions_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "WCA",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReportSyncSignupSubmissions_ActionstepOrgKey",
                schema: "WCA",
                table: "ReportSyncSignupSubmissions",
                column: "ActionstepOrgKey");

            migrationBuilder.CreateIndex(
                name: "IX_ReportSyncSignupSubmissions_CreatedById",
                schema: "WCA",
                table: "ReportSyncSignupSubmissions",
                column: "CreatedById");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.DropTable(
                name: "ReportSyncSignupSubmissions",
                schema: "WCA");
        }
    }
}
