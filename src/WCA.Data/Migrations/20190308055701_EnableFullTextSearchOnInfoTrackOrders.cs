using Microsoft.EntityFrameworkCore.Migrations;

namespace WCA.Data.Migrations
{
    public partial class EnableFullTextSearchOnInfoTrackOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new System.ArgumentNullException(nameof(migrationBuilder));
            }
            // As of EF Core 2.1, there is no available configuration to enable this
            // Enable FullText Index on InfoTrackOrders manually
            migrationBuilder.Sql("CREATE FULLTEXT CATALOG ftCatalog AS DEFAULT; ", suppressTransaction: true);
            migrationBuilder.Sql("CREATE FULLTEXT INDEX ON WCA.InfoTrackOrders(InfoTrackClientReference, InfoTrackDescription) KEY INDEX PK_InfoTrackOrders ON ftCatalog; ", suppressTransaction: true);
            migrationBuilder.Sql("ALTER FULLTEXT INDEX ON WCA.InfoTrackOrders ENABLE; ", suppressTransaction: true);
            migrationBuilder.Sql("ALTER FULLTEXT INDEX ON WCA.InfoTrackOrders START FULL POPULATION; ", suppressTransaction: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new System.ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.Sql("DROP FULLTEXT INDEX ON WCA.InfoTrackOrders; ", suppressTransaction: true);
            migrationBuilder.Sql("DROP FULLTEXT CATALOG ftCatalog; ", suppressTransaction: true);
        }
    }
}
