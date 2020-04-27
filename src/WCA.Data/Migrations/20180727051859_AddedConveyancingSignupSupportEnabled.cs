using Microsoft.EntityFrameworkCore.Migrations;

namespace WCA.Data.Migrations
{
    public partial class AddedConveyancingSignupSupportEnabled : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new System.ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.RenameColumn(
                name: "AcknowledgedFeesAndCharges",
                schema: "WCA",
                table: "ConveyancingSignupSubmissions",
                newName: "SupportPlanEnabled");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ActionstepOrgs_Id",
                schema: "WCA",
                table: "ActionstepOrgs",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new System.ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ActionstepOrgs_Id",
                schema: "WCA",
                table: "ActionstepOrgs");

            migrationBuilder.RenameColumn(
                name: "SupportPlanEnabled",
                schema: "WCA",
                table: "ConveyancingSignupSubmissions",
                newName: "AcknowledgedFeesAndCharges");
        }
    }
}
