using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WCA.Data;
using WCA.Domain.Models.Account;

namespace WCA.UnitTests.TestInfrastructure
{
    public static class TestDataExtensions
    {
        public static string TestUserEmail { get; } = "test@test.domain";
        public static string TestFirstName { get; } = "First";
        public static string TestLastName { get; } = "Last";

        public static string Test2UserEmail { get; } = "test2@test.domain";
        public static string Test2FirstName { get; } = "First2";
        public static string Test2LastName { get; } = "Last2";

        public static async Task SeedTestUsers(this WCADbContext wCADbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetService<UserManager<WCAUser>>();

            var newTestUser = new WCAUser()
            {
                Email = TestUserEmail,
                UserName = TestUserEmail,
                FirstName = TestFirstName,
                LastName = TestLastName
            };

            await userManager.CreateAsync(newTestUser);

            var newTest2User = new WCAUser()
            {
                Email = Test2UserEmail,
                UserName = Test2UserEmail,
                FirstName = Test2FirstName,
                LastName = Test2LastName
            };

            await userManager.CreateAsync(newTest2User);
        }

        public static WCAUser GetTestUser(this WCADbContext dbContext)
        {
            if (dbContext is null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            return dbContext.Users.Single(u => u.NormalizedEmail == TestUserEmail.ToUpper(CultureInfo.InvariantCulture));
        }

        public static async Task<WCAUser> GetTestUser(this WebContainerFixture containerFixture)
        {
            if (containerFixture is null)
            {
                throw new ArgumentNullException(nameof(containerFixture));
            }

            WCAUser testUser = default(WCAUser);

            await containerFixture.ExecuteScopeAsync(async sp =>
            {
                var userManager = sp.GetService<UserManager<WCAUser>>();
                testUser = await userManager.FindByEmailAsync(TestUserEmail);
            });

            return testUser;
        }

        public static async Task<WCAUser> GetTestUser2(this WebContainerFixture containerFixture)
        {
            if (containerFixture is null)
            {
                throw new ArgumentNullException(nameof(containerFixture));
            }

            WCAUser testUser2 = default(WCAUser);

            await containerFixture.ExecuteScopeAsync(async sp =>
            {
                var userManager = sp.GetService<UserManager<WCAUser>>();
                testUser2 = await userManager.FindByEmailAsync(Test2UserEmail);
            });

            return testUser2;
        }
    }
}
