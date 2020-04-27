using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WCA.Data.Migrations
{
    public partial class AddedHouserooCompanyIdAndName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.AddColumn<int>(
                name: "HouserooCompanyId",
                schema: "WCA",
                table: "HouserooCredentials",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "HouserooCompanyName",
                schema: "WCA",
                table: "HouserooCredentials",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.DropColumn(
                name: "HouserooCompanyId",
                schema: "WCA",
                table: "HouserooCredentials");

            migrationBuilder.DropColumn(
                name: "HouserooCompanyName",
                schema: "WCA",
                table: "HouserooCredentials");
        }
    }
}
