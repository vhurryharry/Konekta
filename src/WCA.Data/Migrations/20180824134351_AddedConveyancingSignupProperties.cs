using Microsoft.EntityFrameworkCore.Migrations;

namespace WCA.Data.Migrations
{
    public partial class AddedConveyancingSignupProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new System.ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.AddColumn<string>(
                name: "OrgKey",
                schema: "WCA",
                table: "ConveyancingSignupSubmissions",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PromoCode",
                schema: "WCA",
                table: "ConveyancingSignupSubmissions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new System.ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.DropColumn(
                name: "OrgKey",
                schema: "WCA",
                table: "ConveyancingSignupSubmissions");

            migrationBuilder.DropColumn(
                name: "PromoCode",
                schema: "WCA",
                table: "ConveyancingSignupSubmissions");
        }
    }
}
