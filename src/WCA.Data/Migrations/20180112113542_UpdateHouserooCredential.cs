using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WCA.Data.Migrations
{
    public partial class UpdateHouserooCredential : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.AddColumn<string>(
                name: "ActionstepOrgKey",
                schema: "WCA",
                table: "HouserooCredentials",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HouserooUserEmail",
                schema: "WCA",
                table: "HouserooCredentials",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HouserooUserName",
                schema: "WCA",
                table: "HouserooCredentials",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "HouserooUserRole",
                schema: "WCA",
                table: "HouserooCredentials",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiryUtc",
                schema: "WCA",
                table: "HouserooCredentials",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.DropColumn(
                name: "ActionstepOrgKey",
                schema: "WCA",
                table: "HouserooCredentials");

            migrationBuilder.DropColumn(
                name: "HouserooUserEmail",
                schema: "WCA",
                table: "HouserooCredentials");

            migrationBuilder.DropColumn(
                name: "HouserooUserName",
                schema: "WCA",
                table: "HouserooCredentials");

            migrationBuilder.DropColumn(
                name: "HouserooUserRole",
                schema: "WCA",
                table: "HouserooCredentials");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiryUtc",
                schema: "WCA",
                table: "HouserooCredentials");
        }
    }
}
