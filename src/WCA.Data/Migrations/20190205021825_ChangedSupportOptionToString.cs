using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WCA.Data.Migrations
{
    public partial class ChangedSupportOptionToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.AddColumn<string>(
                name: "SupportPlanOption",
                schema: "WCA",
                table: "ConveyancingSignupSubmissions",
                nullable: true);

            // Copy support option data
            migrationBuilder.Sql(@"
UPDATE [WCA].[ConveyancingSignupSubmissions]
SET [SupportPlanOption] = CASE WHEN [SupportPlanEnabled] = '1' THEN 'Yes' ELSE 'No' END
FROM [WCA].[ConveyancingSignupSubmissions]");

            migrationBuilder.DropColumn(
                name: "SupportPlanEnabled",
                schema: "WCA",
                table: "ConveyancingSignupSubmissions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.AddColumn<bool>(
                name: "SupportPlanEnabled",
                schema: "WCA",
                table: "ConveyancingSignupSubmissions",
                nullable: false,
                defaultValue: false);

            // Copy support option data
            migrationBuilder.Sql(@"
UPDATE [WCA].[ConveyancingSignupSubmissions]
SET [SupportPlanEnabled] = CASE WHEN [SupportPlanOption] = 'Yes' THEN 1 ELSE 0 END
FROM [WCA].[ConveyancingSignupSubmissions]");

            migrationBuilder.DropColumn(
                name: "SupportPlanOption",
                schema: "WCA",
                table: "ConveyancingSignupSubmissions");
        }
    }
}
