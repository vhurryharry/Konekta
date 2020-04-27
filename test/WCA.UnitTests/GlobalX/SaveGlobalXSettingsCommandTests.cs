using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WCA.Core.Features.GlobalX.Sync;
using WCA.Data;
using WCA.Domain.Actionstep;
using WCA.Domain.Models.Account;
using Xunit;

namespace WCA.UnitTests.GlobalX
{
    public class SaveGlobalXSettingsCommandTests
    {
        [Fact]
        public async Task CanSetAdminUserCommand()
        {
            var options = new DbContextOptionsBuilder<WCADbContext>()
                .UseInMemoryDatabase(databaseName: nameof(CanSetAdminUserCommand))
                .Options;

            // Run the test against one instance of the context
            using (var context = new WCADbContextTransient(options))
            {
                var globalXUser = context.Users.Add(new WCAUser());
                var actionstepSyncUser = context.Users.Add(new WCAUser());
                var org = context.ActionstepOrgs.Add(new ActionstepOrg() { Key = "OrgKey" });
                await context.SaveChangesAsync();

                var setAdminUserCommand = new SaveGlobalXSettingsCommand()
                {
                    ActionstepOrgKey = org.Entity.Key,
                    GlobalXAdminId = globalXUser.Entity.Id,
                    ActionstepSyncUserId = actionstepSyncUser.Entity.Id,
                    TransactionSyncEnabled = false,
                    DocumentSyncEnabled = false
                };

                var handler = new SaveGlobalXSettingsCommand.SetAdminUserCommandHandler(context, new SaveGlobalXSettingsCommand.Validator());

                // === Act ===
                await handler.Handle(setAdminUserCommand, new CancellationToken());

                // === Assert ===
                var result = context.GlobalXOrgSettings.Where(g => g.ActionstepOrgKey == org.Entity.Key).Single();
                Assert.Equal(globalXUser.Entity.Id, result.GlobalXAdminId);
                Assert.Equal(actionstepSyncUser.Entity.Id, result.ActionstepSyncUserId);
                Assert.False(result.TransactionSyncEnabled);
                Assert.False(result.DocumentSyncEnabled);
            }
        }
    }
}