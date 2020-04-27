using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WCA.Data.Migrations
{
    public partial class AddedConcurrencyStampToASCredentials : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null) throw new ArgumentNullException(nameof(migrationBuilder));

            migrationBuilder.AddColumn<Guid>(
                name: "ConcurrencyStamp",
                schema: "WCA",
                table: "ActionstepCredentials",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null) throw new ArgumentNullException(nameof(migrationBuilder));

            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                schema: "WCA",
                table: "ActionstepCredentials");
        }
    }
}
