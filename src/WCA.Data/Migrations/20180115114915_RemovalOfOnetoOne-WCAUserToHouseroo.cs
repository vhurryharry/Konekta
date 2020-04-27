using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WCA.Data.Migrations
{
    public partial class RemovalOfOnetoOneWCAUserToHouseroo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.DropForeignKey(
                name: "FK_HouserooCredentials_AspNetUsers_OwnerKey",
                schema: "WCA",
                table: "HouserooCredentials");

            migrationBuilder.DropIndex(
                name: "IX_HouserooCredentials_OwnerKey",
                schema: "WCA",
                table: "HouserooCredentials");

            migrationBuilder.DropColumn(
                name: "OwnerKey",
                schema: "WCA",
                table: "HouserooCredentials");

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                schema: "WCA",
                table: "HouserooCredentials",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_HouserooCredentials_OwnerId",
                schema: "WCA",
                table: "HouserooCredentials",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_HouserooCredentials_AspNetUsers_OwnerId",
                schema: "WCA",
                table: "HouserooCredentials",
                column: "OwnerId",
                principalSchema: "WCA",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.DropForeignKey(
                name: "FK_HouserooCredentials_AspNetUsers_OwnerId",
                schema: "WCA",
                table: "HouserooCredentials");

            migrationBuilder.DropIndex(
                name: "IX_HouserooCredentials_OwnerId",
                schema: "WCA",
                table: "HouserooCredentials");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                schema: "WCA",
                table: "HouserooCredentials");

            migrationBuilder.AddColumn<string>(
                name: "OwnerKey",
                schema: "WCA",
                table: "HouserooCredentials",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HouserooCredentials_OwnerKey",
                schema: "WCA",
                table: "HouserooCredentials",
                column: "OwnerKey",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HouserooCredentials_AspNetUsers_OwnerKey",
                schema: "WCA",
                table: "HouserooCredentials",
                column: "OwnerKey",
                principalSchema: "WCA",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
