using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WCA.Data.Migrations
{
    public partial class UpdatedDefaultDataDates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null) throw new ArgumentNullException(nameof(migrationBuilder));

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "ActionstepOrgs",
                keyColumn: "Key",
                keyValue: "AllOrgsKey",
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "AllUsersId",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "a422a643-d96c-4b00-a4fa-9d1ca046a11f", "929f9124-f4ad-4131-9a4f-d826301d3e3b" });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "IntegrationLinkSettings",
                keyColumn: "Id",
                keyValue: new Guid("0bcd13c4-8320-464d-bd12-960c5aea1a18"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "IntegrationLinkSettings",
                keyColumn: "Id",
                keyValue: new Guid("523d34f6-6151-4b2e-834c-7b1a34fbd6bf"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "IntegrationLinkSettings",
                keyColumn: "Id",
                keyValue: new Guid("98c2c667-7fcf-4b35-be84-10e7dcf85311"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "IntegrationLinkSettings",
                keyColumn: "Id",
                keyValue: new Guid("a0910127-adc7-43dd-958c-9801b7af09d8"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "IntegrationLinkSettings",
                keyColumn: "Id",
                keyValue: new Guid("b03e1c2c-0f25-47f6-857f-c29a7cee1f4a"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "IntegrationLinkSettings",
                keyColumn: "Id",
                keyValue: new Guid("e35020dc-c06c-49c6-8328-ea2b07bda219"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "IntegrationLinks",
                keyColumn: "Id",
                keyValue: new Guid("03a39a32-5fd3-4f09-a616-1fab239725ca"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "IntegrationLinks",
                keyColumn: "Id",
                keyValue: new Guid("4b418018-0fb8-47b3-ac0d-0384e983ffc3"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "IntegrationLinks",
                keyColumn: "Id",
                keyValue: new Guid("9de6f462-cd7d-4776-b253-ff5c170e9120"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "IntegrationLinks",
                keyColumn: "Id",
                keyValue: new Guid("abb6d9db-516d-4e14-bec7-a34ce8e1dcfb"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "IntegrationLinks",
                keyColumn: "Id",
                keyValue: new Guid("bef737bc-7e50-4d6f-b662-8ae2fc511b76"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "IntegrationLinks",
                keyColumn: "Id",
                keyValue: new Guid("c4239316-39a5-4b44-9918-a73a223fe6df"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "IntegrationLinks",
                keyColumn: "Id",
                keyValue: new Guid("d472cf72-6346-4cfc-9e3b-3e2efb5f19d2"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "IntegrationLinks",
                keyColumn: "Id",
                keyValue: new Guid("da695c2e-0fd6-4bea-a845-4f27ecf686c5"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "IntegrationLinks",
                keyColumn: "Id",
                keyValue: new Guid("e2648bf7-f4d4-48c3-8cf7-16338196d991"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "IntegrationSettings",
                keyColumn: "Id",
                keyValue: new Guid("0785f5b2-1a61-478b-a049-12f1c78ed155"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "IntegrationSettings",
                keyColumn: "Id",
                keyValue: new Guid("09178645-669f-4d5d-9055-52166a6c9c23"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "IntegrationSettings",
                keyColumn: "Id",
                keyValue: new Guid("10db2716-631e-4445-a6ff-37c8f7e02d26"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "IntegrationSettings",
                keyColumn: "Id",
                keyValue: new Guid("8c3da309-cd33-4655-85ff-097fc4f1dfd2"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "IntegrationSettings",
                keyColumn: "Id",
                keyValue: new Guid("cb2c8dac-3177-4b23-894e-0833b5840b86"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "IntegrationSettings",
                keyColumn: "Id",
                keyValue: new Guid("f6a6daa1-096a-41f3-8357-82843da3555a"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "Integrations",
                keyColumn: "Id",
                keyValue: new Guid("40760360-a77c-4a5a-af9e-b03321909e80"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "Integrations",
                keyColumn: "Id",
                keyValue: new Guid("4b5c4f21-ad76-4847-96a5-c067865fff5b"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "Integrations",
                keyColumn: "Id",
                keyValue: new Guid("5f9c8e97-deab-42f9-8678-0fb55da7d53c"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "Integrations",
                keyColumn: "Id",
                keyValue: new Guid("d89cc4ac-e709-41a1-aa08-dd47c728b88f"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "Integrations",
                keyColumn: "Id",
                keyValue: new Guid("dde37c19-c431-4406-a0c5-2670085b84b0"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "Integrations",
                keyColumn: "Id",
                keyValue: new Guid("f411b5e1-2762-4374-ab81-228b7b13b22a"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null) throw new ArgumentNullException(nameof(migrationBuilder));

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "ActionstepOrgs",
                keyColumn: "Key",
                keyValue: "AllOrgsKey",
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(2020, 3, 23, 2, 56, 50, 149, DateTimeKind.Utc).AddTicks(9535), new DateTime(2020, 3, 23, 2, 56, 50, 149, DateTimeKind.Utc).AddTicks(9535) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "AllUsersId",
                columns: new[] { "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "69120d41-ca2f-4e7e-b15b-241fb3958713", "c8a56f05-f36e-4bb3-aa31-b07b3a0ad83e" });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "IntegrationLinkSettings",
                keyColumn: "Id",
                keyValue: new Guid("0bcd13c4-8320-464d-bd12-960c5aea1a18"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(2020, 3, 23, 2, 56, 50, 198, DateTimeKind.Utc).AddTicks(7877), new DateTime(2020, 3, 23, 2, 56, 50, 198, DateTimeKind.Utc).AddTicks(7877) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "IntegrationLinkSettings",
                keyColumn: "Id",
                keyValue: new Guid("523d34f6-6151-4b2e-834c-7b1a34fbd6bf"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(2020, 3, 23, 2, 56, 50, 198, DateTimeKind.Utc).AddTicks(7877), new DateTime(2020, 3, 23, 2, 56, 50, 198, DateTimeKind.Utc).AddTicks(7877) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "IntegrationLinkSettings",
                keyColumn: "Id",
                keyValue: new Guid("98c2c667-7fcf-4b35-be84-10e7dcf85311"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(2020, 3, 23, 2, 56, 50, 198, DateTimeKind.Utc).AddTicks(7877), new DateTime(2020, 3, 23, 2, 56, 50, 198, DateTimeKind.Utc).AddTicks(7877) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "IntegrationLinkSettings",
                keyColumn: "Id",
                keyValue: new Guid("a0910127-adc7-43dd-958c-9801b7af09d8"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(2020, 3, 23, 2, 56, 50, 198, DateTimeKind.Utc).AddTicks(7877), new DateTime(2020, 3, 23, 2, 56, 50, 198, DateTimeKind.Utc).AddTicks(7877) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "IntegrationLinkSettings",
                keyColumn: "Id",
                keyValue: new Guid("b03e1c2c-0f25-47f6-857f-c29a7cee1f4a"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(2020, 3, 23, 2, 56, 50, 198, DateTimeKind.Utc).AddTicks(7877), new DateTime(2020, 3, 23, 2, 56, 50, 198, DateTimeKind.Utc).AddTicks(7877) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "IntegrationLinkSettings",
                keyColumn: "Id",
                keyValue: new Guid("e35020dc-c06c-49c6-8328-ea2b07bda219"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(2020, 3, 23, 2, 56, 50, 198, DateTimeKind.Utc).AddTicks(7877), new DateTime(2020, 3, 23, 2, 56, 50, 198, DateTimeKind.Utc).AddTicks(7877) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "IntegrationLinks",
                keyColumn: "Id",
                keyValue: new Guid("03a39a32-5fd3-4f09-a616-1fab239725ca"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(2020, 3, 23, 2, 56, 50, 187, DateTimeKind.Utc).AddTicks(265), new DateTime(2020, 3, 23, 2, 56, 50, 187, DateTimeKind.Utc).AddTicks(265) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "IntegrationLinks",
                keyColumn: "Id",
                keyValue: new Guid("4b418018-0fb8-47b3-ac0d-0384e983ffc3"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(2020, 3, 23, 2, 56, 50, 187, DateTimeKind.Utc).AddTicks(265), new DateTime(2020, 3, 23, 2, 56, 50, 187, DateTimeKind.Utc).AddTicks(265) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "IntegrationLinks",
                keyColumn: "Id",
                keyValue: new Guid("9de6f462-cd7d-4776-b253-ff5c170e9120"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(2020, 3, 23, 2, 56, 50, 187, DateTimeKind.Utc).AddTicks(265), new DateTime(2020, 3, 23, 2, 56, 50, 187, DateTimeKind.Utc).AddTicks(265) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "IntegrationLinks",
                keyColumn: "Id",
                keyValue: new Guid("abb6d9db-516d-4e14-bec7-a34ce8e1dcfb"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(2020, 3, 23, 2, 56, 50, 187, DateTimeKind.Utc).AddTicks(265), new DateTime(2020, 3, 23, 2, 56, 50, 187, DateTimeKind.Utc).AddTicks(265) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "IntegrationLinks",
                keyColumn: "Id",
                keyValue: new Guid("bef737bc-7e50-4d6f-b662-8ae2fc511b76"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(2020, 3, 23, 2, 56, 50, 187, DateTimeKind.Utc).AddTicks(265), new DateTime(2020, 3, 23, 2, 56, 50, 187, DateTimeKind.Utc).AddTicks(265) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "IntegrationLinks",
                keyColumn: "Id",
                keyValue: new Guid("c4239316-39a5-4b44-9918-a73a223fe6df"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(2020, 3, 23, 2, 56, 50, 187, DateTimeKind.Utc).AddTicks(265), new DateTime(2020, 3, 23, 2, 56, 50, 187, DateTimeKind.Utc).AddTicks(265) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "IntegrationLinks",
                keyColumn: "Id",
                keyValue: new Guid("d472cf72-6346-4cfc-9e3b-3e2efb5f19d2"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(2020, 3, 23, 2, 56, 50, 187, DateTimeKind.Utc).AddTicks(265), new DateTime(2020, 3, 23, 2, 56, 50, 187, DateTimeKind.Utc).AddTicks(265) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "IntegrationLinks",
                keyColumn: "Id",
                keyValue: new Guid("da695c2e-0fd6-4bea-a845-4f27ecf686c5"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(2020, 3, 23, 2, 56, 50, 187, DateTimeKind.Utc).AddTicks(265), new DateTime(2020, 3, 23, 2, 56, 50, 187, DateTimeKind.Utc).AddTicks(265) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "IntegrationLinks",
                keyColumn: "Id",
                keyValue: new Guid("e2648bf7-f4d4-48c3-8cf7-16338196d991"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(2020, 3, 23, 2, 56, 50, 187, DateTimeKind.Utc).AddTicks(265), new DateTime(2020, 3, 23, 2, 56, 50, 187, DateTimeKind.Utc).AddTicks(265) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "IntegrationSettings",
                keyColumn: "Id",
                keyValue: new Guid("0785f5b2-1a61-478b-a049-12f1c78ed155"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(2020, 3, 23, 2, 56, 50, 194, DateTimeKind.Utc).AddTicks(9883), new DateTime(2020, 3, 23, 2, 56, 50, 194, DateTimeKind.Utc).AddTicks(9883) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "IntegrationSettings",
                keyColumn: "Id",
                keyValue: new Guid("09178645-669f-4d5d-9055-52166a6c9c23"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(2020, 3, 23, 2, 56, 50, 194, DateTimeKind.Utc).AddTicks(9883), new DateTime(2020, 3, 23, 2, 56, 50, 194, DateTimeKind.Utc).AddTicks(9883) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "IntegrationSettings",
                keyColumn: "Id",
                keyValue: new Guid("10db2716-631e-4445-a6ff-37c8f7e02d26"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(2020, 3, 23, 2, 56, 50, 194, DateTimeKind.Utc).AddTicks(9883), new DateTime(2020, 3, 23, 2, 56, 50, 194, DateTimeKind.Utc).AddTicks(9883) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "IntegrationSettings",
                keyColumn: "Id",
                keyValue: new Guid("8c3da309-cd33-4655-85ff-097fc4f1dfd2"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(2020, 3, 23, 2, 56, 50, 194, DateTimeKind.Utc).AddTicks(9883), new DateTime(2020, 3, 23, 2, 56, 50, 194, DateTimeKind.Utc).AddTicks(9883) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "IntegrationSettings",
                keyColumn: "Id",
                keyValue: new Guid("cb2c8dac-3177-4b23-894e-0833b5840b86"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(2020, 3, 23, 2, 56, 50, 194, DateTimeKind.Utc).AddTicks(9883), new DateTime(2020, 3, 23, 2, 56, 50, 194, DateTimeKind.Utc).AddTicks(9883) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "IntegrationSettings",
                keyColumn: "Id",
                keyValue: new Guid("f6a6daa1-096a-41f3-8357-82843da3555a"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(2020, 3, 23, 2, 56, 50, 194, DateTimeKind.Utc).AddTicks(9883), new DateTime(2020, 3, 23, 2, 56, 50, 194, DateTimeKind.Utc).AddTicks(9883) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "Integrations",
                keyColumn: "Id",
                keyValue: new Guid("40760360-a77c-4a5a-af9e-b03321909e80"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(2020, 3, 23, 2, 56, 50, 191, DateTimeKind.Utc).AddTicks(3795), new DateTime(2020, 3, 23, 2, 56, 50, 191, DateTimeKind.Utc).AddTicks(3795) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "Integrations",
                keyColumn: "Id",
                keyValue: new Guid("4b5c4f21-ad76-4847-96a5-c067865fff5b"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(2020, 3, 23, 2, 56, 50, 191, DateTimeKind.Utc).AddTicks(3795), new DateTime(2020, 3, 23, 2, 56, 50, 191, DateTimeKind.Utc).AddTicks(3795) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "Integrations",
                keyColumn: "Id",
                keyValue: new Guid("5f9c8e97-deab-42f9-8678-0fb55da7d53c"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(2020, 3, 23, 2, 56, 50, 191, DateTimeKind.Utc).AddTicks(3795), new DateTime(2020, 3, 23, 2, 56, 50, 191, DateTimeKind.Utc).AddTicks(3795) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "Integrations",
                keyColumn: "Id",
                keyValue: new Guid("d89cc4ac-e709-41a1-aa08-dd47c728b88f"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(2020, 3, 23, 2, 56, 50, 191, DateTimeKind.Utc).AddTicks(3795), new DateTime(2020, 3, 23, 2, 56, 50, 191, DateTimeKind.Utc).AddTicks(3795) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "Integrations",
                keyColumn: "Id",
                keyValue: new Guid("dde37c19-c431-4406-a0c5-2670085b84b0"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(2020, 3, 23, 2, 56, 50, 191, DateTimeKind.Utc).AddTicks(3795), new DateTime(2020, 3, 23, 2, 56, 50, 191, DateTimeKind.Utc).AddTicks(3795) });

            migrationBuilder.UpdateData(
                schema: "WCA",
                table: "Integrations",
                keyColumn: "Id",
                keyValue: new Guid("f411b5e1-2762-4374-ab81-228b7b13b22a"),
                columns: new[] { "DateCreatedUtc", "LastUpdatedUtc" },
                values: new object[] { new DateTime(2020, 3, 23, 2, 56, 50, 191, DateTimeKind.Utc).AddTicks(3795), new DateTime(2020, 3, 23, 2, 56, 50, 191, DateTimeKind.Utc).AddTicks(3795) });
        }
    }
}
