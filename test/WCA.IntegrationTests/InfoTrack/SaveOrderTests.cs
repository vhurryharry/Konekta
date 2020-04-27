using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using WCA.Core;
using WCA.Core.Features.InfoTrack;
using WCA.Data;
using WCA.IntegrationTests.TestInfrastructure;
using Xunit;

namespace WCA.IntegrationTests.InfoTrack
{
    [Collection(WebContainerCollection.WebContainerCollectionName)]
    public class SaveOrderTests
    {
        private readonly WebContainerFixture _containerFixture;

        public SaveOrderTests(WebContainerFixture containerFixture)
        {
            _containerFixture = containerFixture;
        }

        // Need to revisit due to refactoring
        //[Fact]
        //public async Task CanSaveOrderWithZeroFeesAndNoDownloadUrl()
        //{
        //    await _containerFixture.ExecuteScopeAsync(async sp =>
        //    {
        //        var mediator = sp.GetService<IMediator>();
        //        var db = sp.GetService<WCADbContext>();

        //        var wcaTestOrgKey = "wcamaster";

        //        var adminCredentials = db.ActionstepCredentials
        //            .Include(c => c.Owner)
        //            .First(c => c.ActionstepOrg.Key == wcaTestOrgKey);

        //        var wcaUserId = adminCredentials.Owner.Id.ToString();

        //        var command = new UpdateOrder.Command()
        //        {
        //            AvailableOnline = false,
        //            BillingTypeName = "QLD Titles/Plans",
        //            ClientReference = "7",
        //            DateOrdered = DateTime.Parse("2018-08-27T04:33:28.263Z"),
        //            DateCompleted = DateTime.Parse("0001-01-01T00:00:00Z"),
        //            Description = "QLD: Title Search - 37SP110803",
        //            OrderId = 49883531,
        //            ParentOrderId = 0,
        //            OrderedBy = "ActionStep Demo",
        //            Reference = "37SP110803",
        //            RetailerReference = $"WCA_{wcaTestOrgKey}|{wcaUserId}",
        //            RetailerFee = 0.00M,
        //            RetailerFeeGST = 0.00M,
        //            RetailerFeeTotal = 0.00M,
        //            SupplierFee = 0.00M,
        //            SupplierFeeGST = 0.00M,
        //            SupplierFeeTotal = 0.00M,
        //            TotalFee = 0.00M,
        //            TotalFeeGST = 0.00M,
        //            TotalFeeTotal = 0.00M,
        //            ServiceName = "Title Search",
        //            Status = "Complete",
        //            StatusMessage = "The order completed successfully.",
        //            DownloadUrl = null,
        //            OnlineUrl = null,
        //            IsBillable = false,
        //            FileHash = "",
        //            Email = "support@workcloud.com.au"
        //        };

        //        await mediator.Send(command);
        //    });

        //    // TODO, read Actionstep data and ensure written correctly.
        //}

        //[Fact]
        //public async Task CanSaveOrderWithRetailerFeesWithGSTAndNoDownloadUrl()
        //{
        //    await _containerFixture.ExecuteScopeAsync(async sp =>
        //    {
        //        var mediator = sp.GetService<IMediator>();
        //        var db = sp.GetService<WCADbContext>();

        //        var wcaTestOrgKey = "wcamaster";

        //        var adminCredentials = db.ActionstepCredentials
        //            .Include(c => c.Owner)
        //            .First(c => c.ActionstepOrg.Key == wcaTestOrgKey);

        //        var wcaUserId = adminCredentials.Owner.Id.ToString();

        //        var command = new UpdateOrder.Command()
        //        {
        //            AvailableOnline = false,
        //            BillingTypeName = "QLD Titles/Plans",
        //            ClientReference = "7",
        //            DateOrdered = DateTime.Parse("2018-08-27T04:33:28.263Z"),
        //            DateCompleted = DateTime.Parse("0001-01-01T00:00:00Z"),
        //            Description = "QLD: Title Search - 37SP110803",
        //            OrderId = 49883531,
        //            ParentOrderId = 0,
        //            OrderedBy = "ActionStep Demo",
        //            Reference = "37SP110803",
        //            RetailerReference = $"WCA_{wcaTestOrgKey}|{wcaUserId}",
        //            RetailerFee = 1.50M,
        //            RetailerFeeGST = 0.15M,
        //            RetailerFeeTotal = 1.65M,
        //            SupplierFee = 0.00M,
        //            SupplierFeeGST = 0.00M,
        //            SupplierFeeTotal = 0.00M,
        //            TotalFee = 1.50M,
        //            TotalFeeGST = 0.15M,
        //            TotalFeeTotal = 1.65M,
        //            ServiceName = "Title Search",
        //            Status = "Complete",
        //            StatusMessage = "The order completed successfully.",
        //            DownloadUrl = null,
        //            OnlineUrl = null,
        //            IsBillable = false,
        //            FileHash = "",
        //            Email = "support@workcloud.com.au"
        //        };

        //        await mediator.Send(command);
        //    });

        //    // TODO, read Actionstep data and ensure written correctly.
        //}

        //[Fact]
        //public async Task CanSaveOrderWithStatusError()
        //{
        //    await _containerFixture.ExecuteScopeAsync(async sp =>
        //    {
        //        var mediator = sp.GetService<IMediator>();
        //        var db = sp.GetService<WCADbContext>();

        //        var wcaTestOrgKey = "wcamaster";

        //        var adminCredentials = db.ActionstepCredentials
        //            .Include(c => c.Owner)
        //            .First(c => c.ActionstepOrg.Key == wcaTestOrgKey);

        //        var wcaUserId = adminCredentials.Owner.Id.ToString();

        //        var command = new UpdateOrder.Command()
        //        {
        //            AvailableOnline = false,
        //            BillingTypeName = "QLD Titles/Plans",
        //            ClientReference = "7",
        //            DateOrdered = DateTime.Parse("2018-08-27T04:33:28.263Z"),
        //            DateCompleted = DateTime.Parse("0001-01-01T00:00:00Z"),
        //            Description = "QLD: Title Search - 37SP110803",
        //            OrderId = 49883531,
        //            ParentOrderId = 0,
        //            OrderedBy = "ActionStep Demo",
        //            Reference = "37SP110803",
        //            RetailerReference = $"WCA_{wcaTestOrgKey}|{wcaUserId}",
        //            RetailerFee = 0.00M,
        //            RetailerFeeGST = 0.00M,
        //            RetailerFeeTotal = 0.00M,
        //            SupplierFee = 0.00M,
        //            SupplierFeeGST = 0.00M,
        //            SupplierFeeTotal = 0.00M,
        //            TotalFee = 0.00M,
        //            TotalFeeGST = 0.00M,
        //            TotalFeeTotal = 0.00M,
        //            ServiceName = "Title Search",
        //            Status = "Error",
        //            StatusMessage = "No Title records found matching selection.\n\nErrors occurred. Data was not processed.",
        //            DownloadUrl = null,
        //            OnlineUrl = null,
        //            IsBillable = false,
        //            FileHash = "",
        //            Email = "support@workcloud.com.au"
        //        };

        //        await mediator.Send(command);
        //    });

        //    // TODO, read Actionstep data and ensure written correctly.
        //}
    }
}
