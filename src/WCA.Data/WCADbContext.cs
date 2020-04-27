using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WCA.Domain.Abstractions;
using WCA.Domain.Actionstep;
using WCA.Domain.GlobalX;
using WCA.Domain.InfoTrack;
using WCA.Domain.Integrations;
using WCA.Domain.Models;
using WCA.Domain.Models.Account;
using WCA.Domain.Models.Settlement;

namespace WCA.Data
{
    public class WCADbContext : IdentityDbContext<WCAUser>
    {
        public DbSet<ConveyancingSignupSubmission> ConveyancingSignupSubmissions { get; set; }
        public DbSet<ReportSyncSignupSubmission> ReportSyncSignupSubmissions { get; set; }
        public DbSet<ActionstepCredential> ActionstepCredentials { get; set; }
        public DbSet<ActionstepOrg> ActionstepOrgs { get; set; }
        public DbSet<InfoTrackOrderUpdateMessage> InfoTrackOrderUpdateMessageHistory { get; set; }
        public DbSet<InfoTrackOrder> InfoTrackOrders { get; set; }
        public DbSet<SettlementMatter> SettlementMatters { get; set; }
        public DbSet<ActionstepCredentialSubstitution> ActionstepCredentialSubstitutions { get; set; }
        public DbSet<PexaWorkspace> PexaWorkspaces { get; set; }
        public DbSet<GlobalXOrgSettings> GlobalXOrgSettings { get; set; }
        public DbSet<GlobalXTransactionState> GlobalXTransactionStates { get; set; }
        public DbSet<GlobalXDocumentVersionState> GlobalXDocumentVersionStates { get; set; }
        public DbSet<Integration> Integrations { get; set; }
        public DbSet<IntegrationLink> IntegrationLinks { get; set; }
        public DbSet<IntegrationSetting> IntegrationSettings { get; set; }
        public DbSet<IntegrationLinkSetting> IntegrationLinkSettings { get; set; }
        public DbSet<GlobalXMatterMapping> GlobalXMatterMappings { get; set; }

        public WCADbContext(DbContextOptions<WCADbContext> options) : base(options)
        {
        }

        public WCADbContext() : base() { }

        /// <summary>
        /// Useful references:
        /// - Value Conversions: https://docs.microsoft.com/en-us/ef/core/modeling/value-conversions
        /// </summary>
        /// <param name="builder"></param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            if (builder is null) throw new ArgumentNullException(nameof(builder));

            var jwtConverter = new ValueConverter<JwtSecurityToken, string>(v => v.RawData, v => new JwtSecurityToken(v));
            var relativeOrAbsoluteUriConverter = new ValueConverter<Uri, string>(v => v.ToString(), v => new Uri(v, UriKind.RelativeOrAbsolute));
            var absoluteUriConverter = new ValueConverter<Uri, string>(v => v.ToString(), v => new Uri(v, UriKind.Absolute));

            base.OnModelCreating(builder);
            builder.HasDefaultSchema("WCA");

            // Set all DateTime properties to return DateTimeKind.Utc
            // https://github.com/dotnet/efcore/issues/4711
            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(v => v, v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
            var nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(v => v, v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

            // Set converters based on type
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime))
                    {
                        property.SetValueConverter(dateTimeConverter);
                    }
                    else if (property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(nullableDateTimeConverter);
                    }
                }
            }

            builder.Entity<WCAUser>(entity =>
            {
                var created = DateTime.UtcNow;
                entity.HasData(new WCAUser()
                {
                    Id = WCAUser.AllUsersId,
                    Email = WCAUser.AllUsersId,
                    NormalizedEmail = WCAUser.AllUsersId.ToUpper(CultureInfo.InvariantCulture),
                    UserName = WCAUser.AllUsersId,
                    NormalizedUserName = WCAUser.AllUsersId.ToUpper(CultureInfo.InvariantCulture),
                    FirstName = "All",
                    LastName = "Users",
                    LockoutEnabled = true,
                    LockoutEnd = DateTimeOffset.MaxValue,
                    ConcurrencyStamp = "a422a643-d96c-4b00-a4fa-9d1ca046a11f",
                    SecurityStamp = "929f9124-f4ad-4131-9a4f-d826301d3e3b",
                });
            });

            // Configure after generic type based converters, to give the opportunity for
            // property settings to be overridden by specific configuration if needed.
            ConfigureActionstepOrg(builder);
            ConfigureInfoTrackOrder(builder);
            ConfigureInfoTrackOrderUpdateMessage(builder);
            ConfigureSettlementMatter(builder);
            ConfigurePexaWorkspace(builder);
            ConfigureGlobalXOrgSettings(builder);
            ConfigureGlobalXTransactionState(builder);
            ConfigureGlobalXDocumentVersionState(builder);
            ConfigureActionstepCredential(builder, jwtConverter, absoluteUriConverter);
            ConfigureIntegrationLink(builder);
            ConfigureIntegration(builder);
            ConfigureIntegrationSetting(builder);
            ConfigureIntegrationLinkSetting(builder);
            ConfigureGlobalXMatterMapping(builder);
        }

        private void ConfigureGlobalXMatterMapping(ModelBuilder builder)
        {
            builder.Entity<GlobalXMatterMapping>(entity =>
            {
                entity.HasKey(m => new { m.ActionstepOrgKey, m.GlobalXMatterId });
            });
        }

        private void ConfigureIntegrationSetting(ModelBuilder builder)
        {
            builder.Entity<IntegrationSetting>(entity =>
            {
                entity.Property(s => s.Id).ValueGeneratedOnAdd();
                entity.HasIndex(s => new { s.ActionstepOrgKey, s.UserId });
                entity.HasData(IntegrationsDefaults.IntegrationSettings);
            });
        }

        private void ConfigureIntegrationLinkSetting(ModelBuilder builder)
        {
            builder.Entity<IntegrationLinkSetting>(entity =>
            {
                entity.Property(s => s.Id).ValueGeneratedOnAdd(); 
                entity.HasIndex(s => new { s.ActionstepOrgKey, s.UserId });
                entity.HasData(IntegrationsDefaults.IntegrationLinkSettings);
            });
        }

        private void ConfigureIntegrationLink(ModelBuilder builder)
        {
            builder.Entity<IntegrationLink>(entity =>
            {
                entity.Property(i => i.Id).ValueGeneratedOnAdd();
                entity.HasIndex(l => l.Title);
                entity.HasData(IntegrationsDefaults.IntegrationLinks);
            });
        }

        private void ConfigureIntegration(ModelBuilder builder)
        {
            builder.Entity<Integration>(entity =>
            {
                entity.Property(i => i.Id).ValueGeneratedOnAdd();
                entity.HasIndex(i => i.Title);
                entity.HasData(IntegrationsDefaults.Integrations);
            });
        }


        private static void ConfigureActionstepCredential(ModelBuilder builder, ValueConverter<JwtSecurityToken, string> jwtConverter, ValueConverter<Uri, string> absoluteUriConverter)
        {
            builder.Entity<ActionstepCredential>().Property(e => e.ConcurrencyStamp).IsConcurrencyToken();
            builder.Entity<ActionstepCredential>().Property(e => e.IdToken).HasConversion(jwtConverter);
            builder.Entity<ActionstepCredential>().Property(e => e.ApiEndpoint).HasConversion(absoluteUriConverter);
        }

        private static void ConfigureActionstepOrg(ModelBuilder builder)
        {
            builder.Entity<ActionstepOrg>(entity =>
            {
                entity.HasKey(e => e.Key);
                entity.HasData(ActionstepDefaults.ActionstepOrgs);
            });
        }

        private static void ConfigureInfoTrackOrder(ModelBuilder builder)
        {
            builder.Entity<InfoTrackOrder>().HasKey(e => e.InfoTrackOrderId);
            builder.Entity<InfoTrackOrder>().Property(e => e.InfoTrackOrderId).ValueGeneratedNever();
            builder.Entity<InfoTrackOrder>()
                .Property(e => e.ActionstepDisbursementStatus)
                .HasConversion(new EnumToStringConverter<ActionstepDisbursementStatus>());
            builder.Entity<InfoTrackOrder>()
                .Property(e => e.ActionstepDocumentUploadStatus)
                .HasConversion(new EnumToStringConverter<ActionstepDocumentUploadStatus>());
        }

        private static void ConfigureInfoTrackOrderUpdateMessage(ModelBuilder builder)
        {
            builder.Entity<InfoTrackOrderUpdateMessage>()
                            .Property(e => e.ProcessingStatus)
                            .HasConversion(new EnumToStringConverter<Domain.InfoTrack.ProcessingStatus>());
        }

        private static void ConfigureSettlementMatter(ModelBuilder builder)
        {
            builder.Entity<SettlementMatter>()
                            .Property(e => e.ActionstepData)
                            .HasConversion(
                                v => v.ToString(),
                                v => Domain.Models.Settlement.ActionstepMatter.FromString(v));

            builder.Entity<SettlementMatter>()
                .Property(e => e.SettlementData)
                .HasConversion(
                    v => v.ToString(),
                    v => SettlementInfo.FromString(v));
        }

        private static void ConfigurePexaWorkspace(ModelBuilder builder)
        {
            builder.Entity<PexaWorkspace>()
                            .Property(e => e.WorkspaceUri)
                            .HasConversion(v => v.ToString(), v => new Uri(v));
        }

        private static void ConfigureGlobalXOrgSettings(ModelBuilder builder)
        {
            builder.Entity<GlobalXOrgSettings>(entity =>
            {
                entity.HasKey(g => g.ActionstepOrgKey);
                entity.HasOne(o => o.ActionstepOrg)
                    .WithOne()
                    .HasForeignKey<GlobalXOrgSettings>(g => g.ActionstepOrgKey);
            });
        }

        private static void ConfigureGlobalXTransactionState(ModelBuilder builder)
        {
            builder.Entity<GlobalXTransactionState>(entity =>
            {
                entity.HasKey(t => t.TransactionId);
                entity.Property(t => t.TransactionId)
                    .ValueGeneratedNever();

                entity.Property(t => t.ProcessingStatus)
                    .HasConversion(new EnumToStringConverter<TransactionProcessingStatus>());

                entity.Property(t => t.WholesaleGst).HasColumnType("decimal(18, 2)");
                entity.Property(t => t.WholesalePrice).HasColumnType("decimal(18, 2)");
                entity.Property(t => t.RetailGst).HasColumnType("decimal(18, 2)");
                entity.Property(t => t.RetailPrice).HasColumnType("decimal(18, 2)");

                entity.HasIndex(t => t.OrderId);
                entity.HasIndex(t => t.ProcessingStatus);
                entity.HasIndex(t => t.ActionstepOrgKey);
                entity.HasIndex(t => t.MatterId);
            });
        }

        private static void ConfigureGlobalXDocumentVersionState(ModelBuilder builder)
        {
            builder.Entity<GlobalXDocumentVersionState>(entity =>
            {
                entity.HasKey(dv => dv.DocumentVersionId);
                entity.Property(dv => dv.DocumentVersionId)
                    .ValueGeneratedNever();

                entity.Property(dv => dv.DocumentCopyStatus)
                    .HasConversion(new EnumToStringConverter<DocumentCopyStatus>());

                entity.HasIndex(dv => dv.OrderId);
                entity.HasIndex(dv => dv.DocumentId);
                entity.HasIndex(dv => dv.DocumentCopyStatus);
                entity.HasIndex(dv => dv.ActionstepOrgKey);
                entity.HasIndex(dv => dv.MatterId);
            });
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            UpdateTrackedEntity();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            UpdateTrackedEntity();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        private void UpdateTrackedEntity()
        {
            // So we have the same date/time across entities/columns
            var utcNow = DateTime.UtcNow;

            foreach (var entityEntry in ChangeTracker.Entries())
            {
                if (entityEntry.Entity is ITrackedEntity &&
                    (entityEntry.State == EntityState.Added || entityEntry.State == EntityState.Modified))
                {
                    ((ITrackedEntity)entityEntry.Entity).LastUpdatedUtc = utcNow;
                    if (entityEntry.State == EntityState.Added)
                    {
                        ((ITrackedEntity)entityEntry.Entity).DateCreatedUtc = utcNow;
                    }
                }
            }
        }

        public void Seed()
        {
            throw new NotImplementedException();
        }

        public void SeedTestData()
        {
            var user = Users.FirstOrDefault();
            var org = ActionstepOrgs.FirstOrDefault();

            // Seed data isn't helpful without user/org info.
            if (user == null || org == null)
            {
                return;
            }

            for (int i = 0; i < 25; i++)
            {
                for (int x = 1; x <= 5; x++)
                {
                    var index = i * 5 + x;
                    var matterId = (i + 1) * 1000;
                    var order = InfoTrackOrders.SingleOrDefault(o => o.InfoTrackOrderId == index);
                    if (order == null)
                    {
                        InfoTrackOrders.Add(new InfoTrackOrder
                        {
                            ActionstepDisbursementStatus = ActionstepDisbursementStatus.CreatedSuccessfully,
                            ActionstepDisbursementStatusUpdatedUtc = DateTime.UtcNow,
                            ActionstepDocumentUploadStatus = ActionstepDocumentUploadStatus.UploadedSuccessfully,
                            InfoTrackIsBillable = true,
                            InfoTrackFileHash = "",
                            InfoTrackEmail = "dev@konekta.com.au",
                            ActionstepDocumentUploadStatusUpdatedUtc = DateTime.UtcNow,
                            ActionstepMatterId = matterId,
                            ActionstepOrg = org,
                            CreatedBy = user,
                            DateCreatedUtc = DateTime.UtcNow,
                            InfoTrackAvailableOnline = true,
                            InfoTrackBillingTypeName = "billing-type-name",
                            InfoTrackClientReference = matterId.ToString(CultureInfo.InvariantCulture),
                            InfoTrackDateCompletedUtc = DateTime.UtcNow,
                            InfoTrackDateOrderedUtc = DateTime.UtcNow,
                            InfoTrackDescription = "description",
                            InfoTrackDownloadUrl = "http://www.google.com",
                            InfoTrackOnlineUrl = "http://www.google.com",
                            InfoTrackOrderedBy = "orderedBy",
                            InfoTrackOrderId = index,
                            InfoTrackParentOrderId = 0,
                            InfoTrackReference = "reference",
                            InfoTrackRetailerFee = 10,
                            InfoTrackRetailerFeeGST = 5,
                            InfoTrackRetailerFeeTotal = 15,
                            InfoTrackRetailerReference = "retailReference",
                            InfoTrackServiceName = "serviceName",
                            InfoTrackStatus = "Complete",
                            InfoTrackStatusMessage = "message",
                            InfoTrackSupplierFee = 10,
                            InfoTrackSupplierFeeGST = 5,
                            InfoTrackSupplierFeeTotal = 15,
                            InfoTrackTotalFee = 20,
                            InfoTrackTotalFeeGST = 10,
                            InfoTrackTotalFeeTotal = 30,
                            LastUpdatedUtc = DateTime.UtcNow,
                            OrderedByWCAUser = user,
                            Reconciled = true,
                            UpdatedBy = user
                        });
                    }
                }
            }

            SaveChanges();
        }
    }
}
