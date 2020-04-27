using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WCA.Data.Migrations
{
    public partial class AddTitleReferenceToIncompleteOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.AddColumn<string>(
                name: "TitleReference",
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
                name: "TitleReference",
                schema: "WCA",
                table: "IncompleteOrders");
        }
    }
}
