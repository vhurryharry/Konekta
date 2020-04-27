using Microsoft.EntityFrameworkCore.Migrations;

namespace WCA.Data.Migrations
{
    public partial class AddedValueConverterToInfoTrackHistoryProcessingStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new System.ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.AlterColumn<string>(
                name: "ProcessingStatus",
                schema: "WCA",
                table: "InfoTrackOrderUpdateMessageHistory",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new System.ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.AlterColumn<int>(
                name: "ProcessingStatus",
                schema: "WCA",
                table: "InfoTrackOrderUpdateMessageHistory",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
