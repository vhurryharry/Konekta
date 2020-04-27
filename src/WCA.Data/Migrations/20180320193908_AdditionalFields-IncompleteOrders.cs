using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WCA.Data.Migrations
{
    public partial class AdditionalFieldsIncompleteOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.AddColumn<int>(
                name: "MatterId",
                schema: "WCA",
                table: "IncompleteOrders",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MatterType",
                schema: "WCA",
                table: "IncompleteOrders",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PropertyIdentifier",
                schema: "WCA",
                table: "IncompleteOrders",
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
                name: "MatterId",
                schema: "WCA",
                table: "IncompleteOrders");

            migrationBuilder.DropColumn(
                name: "MatterType",
                schema: "WCA",
                table: "IncompleteOrders");

            migrationBuilder.DropColumn(
                name: "PropertyIdentifier",
                schema: "WCA",
                table: "IncompleteOrders");
        }
    }
}
