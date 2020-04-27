﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WCA.Data;

namespace WCA.Data.Migrations
{
    [DbContext(typeof(WCADbContext))]
    [Migration("20191027082909_AddedActionstepCredentialRevokedAtUtcColumn")]
    partial class AddedActionstepCredentialRevokedAtUtcColumn
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("WCA")
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("WCA.Domain.Actionstep.ActionstepCredential", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccessToken")
                        .IsRequired();

                    b.Property<DateTime>("AccessTokenExpiryUtc");

                    b.Property<string>("ActionstepOrgKey")
                        .IsRequired();

                    b.Property<string>("ApiEndpoint")
                        .IsRequired();

                    b.Property<string>("CreatedById");

                    b.Property<DateTime>("DateCreatedUtc");

                    b.Property<int>("ExpiresIn");

                    b.Property<string>("IdToken");

                    b.Property<DateTime>("LastUpdatedUtc");

                    b.Property<DateTime>("LockExpiresAtUtc");

                    b.Property<Guid>("LockId");

                    b.Property<string>("OwnerId")
                        .IsRequired();

                    b.Property<DateTime>("ReceivedAtUtc");

                    b.Property<string>("RefreshToken");

                    b.Property<DateTime>("RefreshTokenExpiryUtc");

                    b.Property<DateTime?>("RevokedAtUtc");

                    b.Property<string>("TokenType")
                        .IsRequired();

                    b.Property<string>("UpdatedById");

                    b.HasKey("Id");

                    b.HasIndex("ActionstepOrgKey");

                    b.HasIndex("CreatedById");

                    b.HasIndex("OwnerId");

                    b.HasIndex("UpdatedById");

                    b.ToTable("ActionstepCredentials");
                });

            modelBuilder.Entity("WCA.Domain.Actionstep.ActionstepCredentialSubstitution", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedById");

                    b.Property<DateTime>("DateCreatedUtc");

                    b.Property<string>("ForOwnerId");

                    b.Property<DateTime>("LastUpdatedUtc");

                    b.Property<string>("SubstituteWithOwnerId");

                    b.Property<string>("UpdatedById");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("ForOwnerId");

                    b.HasIndex("SubstituteWithOwnerId");

                    b.HasIndex("UpdatedById");

                    b.ToTable("ActionstepCredentialSubstitutions");
                });

            modelBuilder.Entity("WCA.Domain.Actionstep.ActionstepOrg", b =>
                {
                    b.Property<string>("Key")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedById");

                    b.Property<DateTime>("DateCreatedUtc");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("LastUpdatedUtc");

                    b.Property<string>("Title");

                    b.Property<string>("UpdatedById");

                    b.HasKey("Key");

                    b.HasAlternateKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("UpdatedById");

                    b.ToTable("ActionstepOrgs");
                });

            modelBuilder.Entity("WCA.Domain.InfoTrack.InfoTrackOrder", b =>
                {
                    b.Property<int>("InfoTrackOrderId");

                    b.Property<string>("ActionstepDisbursementStatus")
                        .IsRequired();

                    b.Property<DateTime>("ActionstepDisbursementStatusUpdatedUtc");

                    b.Property<string>("ActionstepDocumentUploadStatus")
                        .IsRequired();

                    b.Property<DateTime>("ActionstepDocumentUploadStatusUpdatedUtc");

                    b.Property<int>("ActionstepMatterId");

                    b.Property<string>("ActionstepOrgKey");

                    b.Property<string>("CreatedById");

                    b.Property<DateTime>("DateCreatedUtc");

                    b.Property<bool>("InfoTrackAvailableOnline");

                    b.Property<string>("InfoTrackBillingTypeName");

                    b.Property<string>("InfoTrackClientReference");

                    b.Property<DateTime?>("InfoTrackDateCompletedUtc");

                    b.Property<DateTime>("InfoTrackDateOrderedUtc");

                    b.Property<string>("InfoTrackDescription");

                    b.Property<string>("InfoTrackDownloadUrl");

                    b.Property<string>("InfoTrackEmail");

                    b.Property<string>("InfoTrackFileHash");

                    b.Property<bool>("InfoTrackIsBillable");

                    b.Property<string>("InfoTrackOnlineUrl");

                    b.Property<string>("InfoTrackOrderedBy");

                    b.Property<int>("InfoTrackParentOrderId");

                    b.Property<string>("InfoTrackReference");

                    b.Property<decimal>("InfoTrackRetailerFee")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("InfoTrackRetailerFeeGST")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("InfoTrackRetailerFeeTotal")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<string>("InfoTrackRetailerReference");

                    b.Property<string>("InfoTrackServiceName");

                    b.Property<string>("InfoTrackStatus");

                    b.Property<string>("InfoTrackStatusMessage");

                    b.Property<decimal>("InfoTrackSupplierFee")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("InfoTrackSupplierFeeGST")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("InfoTrackSupplierFeeTotal")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("InfoTrackTotalFee")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("InfoTrackTotalFeeGST")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("InfoTrackTotalFeeTotal")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<DateTime>("LastUpdatedUtc");

                    b.Property<string>("OrderedByWCAUserId");

                    b.Property<bool>("Reconciled");

                    b.Property<string>("UpdatedById");

                    b.HasKey("InfoTrackOrderId");

                    b.HasIndex("ActionstepOrgKey");

                    b.HasIndex("CreatedById");

                    b.HasIndex("OrderedByWCAUserId");

                    b.HasIndex("UpdatedById");

                    b.ToTable("InfoTrackOrders");
                });

            modelBuilder.Entity("WCA.Domain.InfoTrack.InfoTrackOrderUpdateMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedById");

                    b.Property<DateTime>("DateCreatedUtc");

                    b.Property<bool>("InfoTrackAvailableOnline");

                    b.Property<string>("InfoTrackBillingTypeName");

                    b.Property<string>("InfoTrackClientReference");

                    b.Property<DateTime?>("InfoTrackDateCompletedUtc");

                    b.Property<DateTime>("InfoTrackDateOrderedUtc");

                    b.Property<string>("InfoTrackDescription");

                    b.Property<string>("InfoTrackDownloadUrl");

                    b.Property<string>("InfoTrackEmail");

                    b.Property<string>("InfoTrackFileHash");

                    b.Property<bool>("InfoTrackIsBillable");

                    b.Property<string>("InfoTrackOnlineUrl");

                    b.Property<int>("InfoTrackOrderId");

                    b.Property<string>("InfoTrackOrderedBy");

                    b.Property<int>("InfoTrackParentOrderId");

                    b.Property<string>("InfoTrackReference");

                    b.Property<decimal>("InfoTrackRetailerFee")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("InfoTrackRetailerFeeGST")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("InfoTrackRetailerFeeTotal")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<string>("InfoTrackRetailerReference");

                    b.Property<string>("InfoTrackServiceName");

                    b.Property<string>("InfoTrackStatus");

                    b.Property<string>("InfoTrackStatusMessage");

                    b.Property<decimal>("InfoTrackSupplierFee")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("InfoTrackSupplierFeeGST")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("InfoTrackSupplierFeeTotal")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("InfoTrackTotalFee")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("InfoTrackTotalFeeGST")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("InfoTrackTotalFeeTotal")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<DateTime>("LastUpdatedUtc");

                    b.Property<string>("ProcessingStatus")
                        .IsRequired();

                    b.Property<DateTime>("ProcessingStatusUpdatedUtc");

                    b.Property<string>("UpdatedById");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("UpdatedById");

                    b.ToTable("InfoTrackOrderUpdateMessageHistory");
                });

            modelBuilder.Entity("WCA.Domain.Models.Account.WCAUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("WCA.Domain.Models.ConveyancingSignupSubmission", b =>
                {
                    b.Property<int>("ConveyancingSignupSubmissionId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ABN");

                    b.Property<bool>("AcceptedTermsAndConditions");

                    b.Property<string>("Address1");

                    b.Property<string>("Address2");

                    b.Property<string>("City");

                    b.Property<string>("Company");

                    b.Property<string>("ConveyancingApp");

                    b.Property<string>("Email");

                    b.Property<string>("Firstname");

                    b.Property<string>("Lastname");

                    b.Property<string>("OrgKey");

                    b.Property<string>("Phone");

                    b.Property<string>("Postcode");

                    b.Property<string>("PromoCode");

                    b.Property<string>("State");

                    b.Property<DateTime>("SubmittedDateTimeUtc");

                    b.Property<string>("SupportPlanOption");

                    b.HasKey("ConveyancingSignupSubmissionId");

                    b.ToTable("ConveyancingSignupSubmissions");
                });

            modelBuilder.Entity("WCA.Domain.Models.ReportSyncSignupSubmission", b =>
                {
                    b.Property<int>("ReportSyncSignupSubmissionId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ABN");

                    b.Property<bool>("AcceptedTermsAndConditions");

                    b.Property<bool>("AcknowledgedFeesAndCharges");

                    b.Property<string>("ActionstepOrgKey");

                    b.Property<string>("Address1");

                    b.Property<string>("Address2");

                    b.Property<string>("BillingContactEmail");

                    b.Property<string>("BillingContactFirstname");

                    b.Property<string>("BillingContactLastname");

                    b.Property<string>("BillingFrequency");

                    b.Property<string>("City");

                    b.Property<string>("Company");

                    b.Property<string>("CreatedById");

                    b.Property<string>("Phone");

                    b.Property<string>("Postcode");

                    b.Property<string>("ServiceContactEmail");

                    b.Property<string>("ServiceContactFirstname");

                    b.Property<string>("ServiceContactLastname");

                    b.Property<string>("State");

                    b.Property<string>("StripeId");

                    b.Property<DateTime>("SubmittedDateTimeUtc");

                    b.HasKey("ReportSyncSignupSubmissionId");

                    b.HasIndex("ActionstepOrgKey");

                    b.HasIndex("CreatedById");

                    b.ToTable("ReportSyncSignupSubmissions");
                });

            modelBuilder.Entity("WCA.Domain.Models.Settlement.SettlementMatter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ActionstepData")
                        .HasColumnType("nvarchar(MAX)");

                    b.Property<int>("ActionstepMatterId");

                    b.Property<string>("ActionstepOrgKey");

                    b.Property<string>("CreatedById");

                    b.Property<DateTime>("DateCreatedUtc");

                    b.Property<DateTime>("LastUpdatedUtc");

                    b.Property<string>("SettlementData")
                        .HasColumnType("nvarchar(MAX)");

                    b.Property<string>("UpdatedById");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("UpdatedById");

                    b.ToTable("SettlementMatters");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("WCA.Domain.Models.Account.WCAUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("WCA.Domain.Models.Account.WCAUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WCA.Domain.Models.Account.WCAUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("WCA.Domain.Models.Account.WCAUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("WCA.Domain.Actionstep.ActionstepCredential", b =>
                {
                    b.HasOne("WCA.Domain.Actionstep.ActionstepOrg", "ActionstepOrg")
                        .WithMany("Credentials")
                        .HasForeignKey("ActionstepOrgKey")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WCA.Domain.Models.Account.WCAUser", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("WCA.Domain.Models.Account.WCAUser", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WCA.Domain.Models.Account.WCAUser", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedById");
                });

            modelBuilder.Entity("WCA.Domain.Actionstep.ActionstepCredentialSubstitution", b =>
                {
                    b.HasOne("WCA.Domain.Models.Account.WCAUser", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("WCA.Domain.Models.Account.WCAUser", "ForOwner")
                        .WithMany()
                        .HasForeignKey("ForOwnerId");

                    b.HasOne("WCA.Domain.Models.Account.WCAUser", "SubstituteWithOwner")
                        .WithMany()
                        .HasForeignKey("SubstituteWithOwnerId");

                    b.HasOne("WCA.Domain.Models.Account.WCAUser", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedById");
                });

            modelBuilder.Entity("WCA.Domain.Actionstep.ActionstepOrg", b =>
                {
                    b.HasOne("WCA.Domain.Models.Account.WCAUser", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("WCA.Domain.Models.Account.WCAUser", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedById");
                });

            modelBuilder.Entity("WCA.Domain.InfoTrack.InfoTrackOrder", b =>
                {
                    b.HasOne("WCA.Domain.Actionstep.ActionstepOrg", "ActionstepOrg")
                        .WithMany()
                        .HasForeignKey("ActionstepOrgKey");

                    b.HasOne("WCA.Domain.Models.Account.WCAUser", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("WCA.Domain.Models.Account.WCAUser", "OrderedByWCAUser")
                        .WithMany()
                        .HasForeignKey("OrderedByWCAUserId");

                    b.HasOne("WCA.Domain.Models.Account.WCAUser", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedById");
                });

            modelBuilder.Entity("WCA.Domain.InfoTrack.InfoTrackOrderUpdateMessage", b =>
                {
                    b.HasOne("WCA.Domain.Models.Account.WCAUser", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("WCA.Domain.Models.Account.WCAUser", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedById");
                });

            modelBuilder.Entity("WCA.Domain.Models.ReportSyncSignupSubmission", b =>
                {
                    b.HasOne("WCA.Domain.Actionstep.ActionstepOrg", "ActionstepOrg")
                        .WithMany()
                        .HasForeignKey("ActionstepOrgKey");

                    b.HasOne("WCA.Domain.Models.Account.WCAUser", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");
                });

            modelBuilder.Entity("WCA.Domain.Models.Settlement.SettlementMatter", b =>
                {
                    b.HasOne("WCA.Domain.Models.Account.WCAUser", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("WCA.Domain.Models.Account.WCAUser", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedById");
                });
#pragma warning restore 612, 618
        }
    }
}
