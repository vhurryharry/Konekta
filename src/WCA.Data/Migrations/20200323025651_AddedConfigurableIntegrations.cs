using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Diagnostics.CodeAnalysis;

namespace WCA.Data.Migrations
{
    public partial class AddedConfigurableIntegrations : Migration
    {
        [SuppressMessage("Performance", "CA1814:Prefer jagged arrays over multidimensional", Justification = "Auto generated")]
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null) throw new ArgumentNullException(nameof(migrationBuilder));

            migrationBuilder.CreateTable(
                name: "Integrations",
                schema: "WCA",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    LastUpdatedUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    LogoSrc = table.Column<string>(nullable: true),
                    LogoAlt = table.Column<string>(nullable: true),
                    LogoHref = table.Column<string>(nullable: true),
                    LogoWidth = table.Column<string>(nullable: true),
                    ComingSoon = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Integrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Integrations_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "WCA",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Integrations_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalSchema: "WCA",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IntegrationLinks",
                schema: "WCA",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    LastUpdatedUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Href = table.Column<string>(nullable: true),
                    OpenInNewWindow = table.Column<bool>(nullable: false),
                    IsReactLink = table.Column<bool>(nullable: false),
                    IsBeta = table.Column<bool>(nullable: false),
                    Disabled = table.Column<bool>(nullable: false),
                    ToolTip = table.Column<string>(nullable: true),
                    IntegrationId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IntegrationLinks_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "WCA",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IntegrationLinks_Integrations_IntegrationId",
                        column: x => x.IntegrationId,
                        principalSchema: "WCA",
                        principalTable: "Integrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IntegrationLinks_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalSchema: "WCA",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IntegrationSettings",
                schema: "WCA",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    LastUpdatedUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    ActionstepOrgKey = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    SortOrder = table.Column<int>(nullable: false),
                    HideIntegration = table.Column<bool>(nullable: false),
                    IntegrationId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IntegrationSettings_ActionstepOrgs_ActionstepOrgKey",
                        column: x => x.ActionstepOrgKey,
                        principalSchema: "WCA",
                        principalTable: "ActionstepOrgs",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IntegrationSettings_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "WCA",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IntegrationSettings_Integrations_IntegrationId",
                        column: x => x.IntegrationId,
                        principalSchema: "WCA",
                        principalTable: "Integrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IntegrationSettings_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalSchema: "WCA",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IntegrationSettings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "WCA",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IntegrationLinkSettings",
                schema: "WCA",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DateCreatedUtc = table.Column<DateTime>(nullable: false),
                    CreatedById = table.Column<string>(nullable: true),
                    LastUpdatedUtc = table.Column<DateTime>(nullable: false),
                    UpdatedById = table.Column<string>(nullable: true),
                    ActionstepOrgKey = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    SortOrder = table.Column<int>(nullable: false),
                    HideIntegrationLink = table.Column<bool>(nullable: false),
                    IntegrationLinkId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationLinkSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IntegrationLinkSettings_ActionstepOrgs_ActionstepOrgKey",
                        column: x => x.ActionstepOrgKey,
                        principalSchema: "WCA",
                        principalTable: "ActionstepOrgs",
                        principalColumn: "Key",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IntegrationLinkSettings_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalSchema: "WCA",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IntegrationLinkSettings_IntegrationLinks_IntegrationLinkId",
                        column: x => x.IntegrationLinkId,
                        principalSchema: "WCA",
                        principalTable: "IntegrationLinks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IntegrationLinkSettings_AspNetUsers_UpdatedById",
                        column: x => x.UpdatedById,
                        principalSchema: "WCA",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_IntegrationLinkSettings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "WCA",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "WCA",
                table: "ActionstepOrgs",
                columns: new[] { "Key", "CreatedById", "DateCreatedUtc", "LastUpdatedUtc", "Title", "UpdatedById" },
                values: new object[] { "AllOrgsKey", null, new DateTime(2020, 3, 23, 2, 56, 50, 149, DateTimeKind.Utc).AddTicks(9535), new DateTime(2020, 3, 23, 2, 56, 50, 149, DateTimeKind.Utc).AddTicks(9535), "All Orgs", null });

            migrationBuilder.InsertData(
                schema: "WCA",
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "AllUsersId", 0, "69120d41-ca2f-4e7e-b15b-241fb3958713", "AllUsersId", false, "All", "Users", true, new DateTimeOffset(new DateTime(9999, 12, 31, 23, 59, 59, 999, DateTimeKind.Unspecified).AddTicks(9999), new TimeSpan(0, 0, 0, 0, 0)), "ALLUSERSID", "ALLUSERSID", null, null, false, "c8a56f05-f36e-4bb3-aa31-b07b3a0ad83e", false, "AllUsersId" });

            migrationBuilder.InsertData(
                schema: "WCA",
                table: "Integrations",
                columns: new[] { "Id", "ComingSoon", "CreatedById", "DateCreatedUtc", "LastUpdatedUtc", "LogoAlt", "LogoHref", "LogoSrc", "LogoWidth", "Title", "UpdatedById" },
                values: new object[,]
                {
                    { new Guid("dde37c19-c431-4406-a0c5-2670085b84b0"), false, null, new DateTime(2020, 3, 23, 2, 56, 50, 191, DateTimeKind.Utc).AddTicks(3795), new DateTime(2020, 3, 23, 2, 56, 50, 191, DateTimeKind.Utc).AddTicks(3795), "PEXA Logo", "https://www.pexa.com.au/", "/images/pexa-logo.svg", "100px", "PEXA", null },
                    { new Guid("5f9c8e97-deab-42f9-8678-0fb55da7d53c"), false, null, new DateTime(2020, 3, 23, 2, 56, 50, 191, DateTimeKind.Utc).AddTicks(3795), new DateTime(2020, 3, 23, 2, 56, 50, 191, DateTimeKind.Utc).AddTicks(3795), "InfoTrack Logo", "https://www.infotrack.com.au/", "/images/InfoTrackLogo_216x80.png", "100px", "InfoTrack", null },
                    { new Guid("d89cc4ac-e709-41a1-aa08-dd47c728b88f"), false, null, new DateTime(2020, 3, 23, 2, 56, 50, 191, DateTimeKind.Utc).AddTicks(3795), new DateTime(2020, 3, 23, 2, 56, 50, 191, DateTimeKind.Utc).AddTicks(3795), "Calculators icon", "https://www.konekta.com.au/", "/images/conveyancing-calculators.png", "80px", "Conveyancing Calculators", null },
                    { new Guid("4b5c4f21-ad76-4847-96a5-c067865fff5b"), true, null, new DateTime(2020, 3, 23, 2, 56, 50, 191, DateTimeKind.Utc).AddTicks(3795), new DateTime(2020, 3, 23, 2, 56, 50, 191, DateTimeKind.Utc).AddTicks(3795), "First Title Logo", "https://www.firsttitle.com.au/", "/images/firsttitle-logo.svg", "100px", "First Title", null },
                    { new Guid("40760360-a77c-4a5a-af9e-b03321909e80"), true, null, new DateTime(2020, 3, 23, 2, 56, 50, 191, DateTimeKind.Utc).AddTicks(3795), new DateTime(2020, 3, 23, 2, 56, 50, 191, DateTimeKind.Utc).AddTicks(3795), "GlobalX Logo", "https://globalx.com.au/", "/images/globalx-logo.png", "150px", "GlobalX", null },
                    { new Guid("f411b5e1-2762-4374-ab81-228b7b13b22a"), true, null, new DateTime(2020, 3, 23, 2, 56, 50, 191, DateTimeKind.Utc).AddTicks(3795), new DateTime(2020, 3, 23, 2, 56, 50, 191, DateTimeKind.Utc).AddTicks(3795), "The Search People Logo", "https://thesearchpeople.com.au/", "/images/the-search-people-logo.png", "150px", "The Search People", null }
                });

            migrationBuilder.InsertData(
                schema: "WCA",
                table: "IntegrationLinks",
                columns: new[] { "Id", "CreatedById", "DateCreatedUtc", "Disabled", "Href", "IntegrationId", "IsBeta", "IsReactLink", "LastUpdatedUtc", "OpenInNewWindow", "Title", "ToolTip", "UpdatedById" },
                values: new object[,]
                {
                    { new Guid("e2648bf7-f4d4-48c3-8cf7-16338196d991"), null, new DateTime(2020, 3, 23, 2, 56, 50, 187, DateTimeKind.Utc).AddTicks(265), false, "/pexa/create-workspace?actionsteporg={actionstepOrg}&matterid={matterId}", new Guid("dde37c19-c431-4406-a0c5-2670085b84b0"), false, true, new DateTime(2020, 3, 23, 2, 56, 50, 187, DateTimeKind.Utc).AddTicks(265), false, "Create Workspace", null, null },
                    { new Guid("bef737bc-7e50-4d6f-b662-8ae2fc511b76"), null, new DateTime(2020, 3, 23, 2, 56, 50, 187, DateTimeKind.Utc).AddTicks(265), false, "/wca/infotrack/redirect-with-matter-info?resolvableEntryPoint=PropertyEnquiry&matterId={matterId}&actionstepOrg={actionstepOrg}", new Guid("5f9c8e97-deab-42f9-8678-0fb55da7d53c"), false, false, new DateTime(2020, 3, 23, 2, 56, 50, 187, DateTimeKind.Utc).AddTicks(265), true, "Property Inquiry", null, null },
                    { new Guid("da695c2e-0fd6-4bea-a845-4f27ecf686c5"), null, new DateTime(2020, 3, 23, 2, 56, 50, 187, DateTimeKind.Utc).AddTicks(265), false, "/wca/infotrack/redirect-with-matter-info?resolvableEntryPoint=TitleSearch&matterId={matterId}&actionstepOrg={actionstepOrg}", new Guid("5f9c8e97-deab-42f9-8678-0fb55da7d53c"), false, false, new DateTime(2020, 3, 23, 2, 56, 50, 187, DateTimeKind.Utc).AddTicks(265), true, "Title Search", null, null },
                    { new Guid("d472cf72-6346-4cfc-9e3b-3e2efb5f19d2"), null, new DateTime(2020, 3, 23, 2, 56, 50, 187, DateTimeKind.Utc).AddTicks(265), false, "/wca/infotrack/orders", new Guid("5f9c8e97-deab-42f9-8678-0fb55da7d53c"), false, false, new DateTime(2020, 3, 23, 2, 56, 50, 187, DateTimeKind.Utc).AddTicks(265), false, "Order History", null, null },
                    { new Guid("abb6d9db-516d-4e14-bec7-a34ce8e1dcfb"), null, new DateTime(2020, 3, 23, 2, 56, 50, 187, DateTimeKind.Utc).AddTicks(265), false, "/api/conveyancing/old-settlement-calculator/redirect-with-matter-data/{actionstepOrg}/{matterId}", new Guid("d89cc4ac-e709-41a1-aa08-dd47c728b88f"), false, false, new DateTime(2020, 3, 23, 2, 56, 50, 187, DateTimeKind.Utc).AddTicks(265), true, "Settlement Calculator", null, null },
                    { new Guid("9de6f462-cd7d-4776-b253-ff5c170e9120"), null, new DateTime(2020, 3, 23, 2, 56, 50, 187, DateTimeKind.Utc).AddTicks(265), false, "/wca/stamp-duty-calculator", new Guid("d89cc4ac-e709-41a1-aa08-dd47c728b88f"), false, false, new DateTime(2020, 3, 23, 2, 56, 50, 187, DateTimeKind.Utc).AddTicks(265), false, "Stamp Duty Calculator", null, null },
                    { new Guid("c4239316-39a5-4b44-9918-a73a223fe6df"), null, new DateTime(2020, 3, 23, 2, 56, 50, 187, DateTimeKind.Utc).AddTicks(265), false, "/firsttitle/request-policy?actionsteporg={actionstepOrg}&matterid={matterId}", new Guid("4b5c4f21-ad76-4847-96a5-c067865fff5b"), true, true, new DateTime(2020, 3, 23, 2, 56, 50, 187, DateTimeKind.Utc).AddTicks(265), false, "Order Policy", null, null },
                    { new Guid("03a39a32-5fd3-4f09-a616-1fab239725ca"), null, new DateTime(2020, 3, 23, 2, 56, 50, 187, DateTimeKind.Utc).AddTicks(265), false, "/globalx/property-information?actionsteporg={actionstepOrg}&matterid={matterId}&entryPoint=propertyinformation&embed=true", new Guid("40760360-a77c-4a5a-af9e-b03321909e80"), true, true, new DateTime(2020, 3, 23, 2, 56, 50, 187, DateTimeKind.Utc).AddTicks(265), true, "Property Information", null, null },
                    { new Guid("4b418018-0fb8-47b3-ac0d-0384e983ffc3"), null, new DateTime(2020, 3, 23, 2, 56, 50, 187, DateTimeKind.Utc).AddTicks(265), false, "/globalx/property-information?actionsteporg={actionstepOrg}&matterid={matterId}", new Guid("40760360-a77c-4a5a-af9e-b03321909e80"), true, true, new DateTime(2020, 3, 23, 2, 56, 50, 187, DateTimeKind.Utc).AddTicks(265), true, "All Products", null, null }
                });

            migrationBuilder.InsertData(
                schema: "WCA",
                table: "IntegrationSettings",
                columns: new[] { "Id", "ActionstepOrgKey", "CreatedById", "DateCreatedUtc", "HideIntegration", "IntegrationId", "LastUpdatedUtc", "SortOrder", "UpdatedById", "UserId" },
                values: new object[,]
                {
                    { new Guid("09178645-669f-4d5d-9055-52166a6c9c23"), "AllOrgsKey", null, new DateTime(2020, 3, 23, 2, 56, 50, 194, DateTimeKind.Utc).AddTicks(9883), false, new Guid("dde37c19-c431-4406-a0c5-2670085b84b0"), new DateTime(2020, 3, 23, 2, 56, 50, 194, DateTimeKind.Utc).AddTicks(9883), 10, null, "AllUsersId" },
                    { new Guid("0785f5b2-1a61-478b-a049-12f1c78ed155"), "AllOrgsKey", null, new DateTime(2020, 3, 23, 2, 56, 50, 194, DateTimeKind.Utc).AddTicks(9883), false, new Guid("5f9c8e97-deab-42f9-8678-0fb55da7d53c"), new DateTime(2020, 3, 23, 2, 56, 50, 194, DateTimeKind.Utc).AddTicks(9883), 20, null, "AllUsersId" },
                    { new Guid("cb2c8dac-3177-4b23-894e-0833b5840b86"), "AllOrgsKey", null, new DateTime(2020, 3, 23, 2, 56, 50, 194, DateTimeKind.Utc).AddTicks(9883), false, new Guid("d89cc4ac-e709-41a1-aa08-dd47c728b88f"), new DateTime(2020, 3, 23, 2, 56, 50, 194, DateTimeKind.Utc).AddTicks(9883), 30, null, "AllUsersId" },
                    { new Guid("f6a6daa1-096a-41f3-8357-82843da3555a"), "AllOrgsKey", null, new DateTime(2020, 3, 23, 2, 56, 50, 194, DateTimeKind.Utc).AddTicks(9883), false, new Guid("4b5c4f21-ad76-4847-96a5-c067865fff5b"), new DateTime(2020, 3, 23, 2, 56, 50, 194, DateTimeKind.Utc).AddTicks(9883), 40, null, "AllUsersId" },
                    { new Guid("10db2716-631e-4445-a6ff-37c8f7e02d26"), "AllOrgsKey", null, new DateTime(2020, 3, 23, 2, 56, 50, 194, DateTimeKind.Utc).AddTicks(9883), false, new Guid("40760360-a77c-4a5a-af9e-b03321909e80"), new DateTime(2020, 3, 23, 2, 56, 50, 194, DateTimeKind.Utc).AddTicks(9883), 50, null, "AllUsersId" },
                    { new Guid("8c3da309-cd33-4655-85ff-097fc4f1dfd2"), "AllOrgsKey", null, new DateTime(2020, 3, 23, 2, 56, 50, 194, DateTimeKind.Utc).AddTicks(9883), false, new Guid("f411b5e1-2762-4374-ab81-228b7b13b22a"), new DateTime(2020, 3, 23, 2, 56, 50, 194, DateTimeKind.Utc).AddTicks(9883), 60, null, "AllUsersId" }
                });

            migrationBuilder.InsertData(
                schema: "WCA",
                table: "IntegrationLinkSettings",
                columns: new[] { "Id", "ActionstepOrgKey", "CreatedById", "DateCreatedUtc", "HideIntegrationLink", "IntegrationLinkId", "LastUpdatedUtc", "SortOrder", "UpdatedById", "UserId" },
                values: new object[,]
                {
                    { new Guid("b03e1c2c-0f25-47f6-857f-c29a7cee1f4a"), "AllOrgsKey", null, new DateTime(2020, 3, 23, 2, 56, 50, 198, DateTimeKind.Utc).AddTicks(7877), false, new Guid("e2648bf7-f4d4-48c3-8cf7-16338196d991"), new DateTime(2020, 3, 23, 2, 56, 50, 198, DateTimeKind.Utc).AddTicks(7877), 10, null, "AllUsersId" },
                    { new Guid("98c2c667-7fcf-4b35-be84-10e7dcf85311"), "AllOrgsKey", null, new DateTime(2020, 3, 23, 2, 56, 50, 198, DateTimeKind.Utc).AddTicks(7877), false, new Guid("bef737bc-7e50-4d6f-b662-8ae2fc511b76"), new DateTime(2020, 3, 23, 2, 56, 50, 198, DateTimeKind.Utc).AddTicks(7877), 10, null, "AllUsersId" },
                    { new Guid("a0910127-adc7-43dd-958c-9801b7af09d8"), "AllOrgsKey", null, new DateTime(2020, 3, 23, 2, 56, 50, 198, DateTimeKind.Utc).AddTicks(7877), false, new Guid("da695c2e-0fd6-4bea-a845-4f27ecf686c5"), new DateTime(2020, 3, 23, 2, 56, 50, 198, DateTimeKind.Utc).AddTicks(7877), 20, null, "AllUsersId" },
                    { new Guid("523d34f6-6151-4b2e-834c-7b1a34fbd6bf"), "AllOrgsKey", null, new DateTime(2020, 3, 23, 2, 56, 50, 198, DateTimeKind.Utc).AddTicks(7877), false, new Guid("d472cf72-6346-4cfc-9e3b-3e2efb5f19d2"), new DateTime(2020, 3, 23, 2, 56, 50, 198, DateTimeKind.Utc).AddTicks(7877), 30, null, "AllUsersId" },
                    { new Guid("0bcd13c4-8320-464d-bd12-960c5aea1a18"), "AllOrgsKey", null, new DateTime(2020, 3, 23, 2, 56, 50, 198, DateTimeKind.Utc).AddTicks(7877), false, new Guid("abb6d9db-516d-4e14-bec7-a34ce8e1dcfb"), new DateTime(2020, 3, 23, 2, 56, 50, 198, DateTimeKind.Utc).AddTicks(7877), 10, null, "AllUsersId" },
                    { new Guid("e35020dc-c06c-49c6-8328-ea2b07bda219"), "AllOrgsKey", null, new DateTime(2020, 3, 23, 2, 56, 50, 198, DateTimeKind.Utc).AddTicks(7877), false, new Guid("9de6f462-cd7d-4776-b253-ff5c170e9120"), new DateTime(2020, 3, 23, 2, 56, 50, 198, DateTimeKind.Utc).AddTicks(7877), 20, null, "AllUsersId" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationLinks_CreatedById",
                schema: "WCA",
                table: "IntegrationLinks",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationLinks_IntegrationId",
                schema: "WCA",
                table: "IntegrationLinks",
                column: "IntegrationId");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationLinks_Title",
                schema: "WCA",
                table: "IntegrationLinks",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationLinks_UpdatedById",
                schema: "WCA",
                table: "IntegrationLinks",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationLinkSettings_CreatedById",
                schema: "WCA",
                table: "IntegrationLinkSettings",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationLinkSettings_IntegrationLinkId",
                schema: "WCA",
                table: "IntegrationLinkSettings",
                column: "IntegrationLinkId");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationLinkSettings_UpdatedById",
                schema: "WCA",
                table: "IntegrationLinkSettings",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationLinkSettings_UserId",
                schema: "WCA",
                table: "IntegrationLinkSettings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationLinkSettings_ActionstepOrgKey_UserId",
                schema: "WCA",
                table: "IntegrationLinkSettings",
                columns: new[] { "ActionstepOrgKey", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_Integrations_CreatedById",
                schema: "WCA",
                table: "Integrations",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Integrations_Title",
                schema: "WCA",
                table: "Integrations",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Integrations_UpdatedById",
                schema: "WCA",
                table: "Integrations",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationSettings_CreatedById",
                schema: "WCA",
                table: "IntegrationSettings",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationSettings_IntegrationId",
                schema: "WCA",
                table: "IntegrationSettings",
                column: "IntegrationId");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationSettings_UpdatedById",
                schema: "WCA",
                table: "IntegrationSettings",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationSettings_UserId",
                schema: "WCA",
                table: "IntegrationSettings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationSettings_ActionstepOrgKey_UserId",
                schema: "WCA",
                table: "IntegrationSettings",
                columns: new[] { "ActionstepOrgKey", "UserId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null) throw new ArgumentNullException(nameof(migrationBuilder));

            migrationBuilder.DropTable(
                name: "IntegrationLinkSettings",
                schema: "WCA");

            migrationBuilder.DropTable(
                name: "IntegrationSettings",
                schema: "WCA");

            migrationBuilder.DropTable(
                name: "IntegrationLinks",
                schema: "WCA");

            migrationBuilder.DropTable(
                name: "Integrations",
                schema: "WCA");

            migrationBuilder.DeleteData(
                schema: "WCA",
                table: "ActionstepOrgs",
                keyColumn: "Key",
                keyValue: "AllOrgsKey");

            migrationBuilder.DeleteData(
                schema: "WCA",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "AllUsersId");
        }
    }
}
