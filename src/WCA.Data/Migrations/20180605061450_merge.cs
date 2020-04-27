using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace WCA.Data.Migrations
{
    public partial class merge : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                schema: "WCA",
                table: "AspNetUsers");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ActionstepOrgs_Id",
                schema: "WCA",
                table: "ActionstepOrgs");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                schema: "WCA",
                table: "AspNetRoles");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "WCA",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "WCA",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                schema: "WCA",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                schema: "WCA",
                table: "AspNetRoles");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ActionstepOrgs_Id",
                schema: "WCA",
                table: "ActionstepOrgs",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "WCA",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "WCA",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);
        }
    }
}
