﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using WCA.Data;

namespace WCA.Data.Migrations
{
    [DbContext(typeof(WCADbContext))]
    [Migration("20171016133832_Added Houseroo Credentials")]
    partial class AddedHouserooCredentials
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasDefaultSchema("WCA")
                .HasAnnotation("ProductVersion", "1.1.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
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
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
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

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
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
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccessToken")
                        .IsRequired();

                    b.Property<DateTime>("AccessTokenExpiryUtc");

                    b.Property<string>("ActionstepOrgKey")
                        .IsRequired();

                    b.Property<string>("CreatedById");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("LastUpdated");

                    b.Property<string>("NameIdentifier")
                        .IsRequired();

                    b.Property<string>("OwnerId")
                        .IsRequired();

                    b.Property<string>("RefreshToken");

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

            modelBuilder.Entity("WCA.Domain.Actionstep.ActionstepOrg", b =>
                {
                    b.Property<string>("Key")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedById");

                    b.Property<DateTime>("DateCreated");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("LastUpdated");

                    b.Property<string>("Title");

                    b.Property<string>("UpdatedById");

                    b.HasKey("Key");

                    b.HasAlternateKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("UpdatedById");

                    b.ToTable("ActionstepOrgs");
                });

            modelBuilder.Entity("WCA.Domain.Houseroo.HouserooCredential", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccessToken")
                        .IsRequired();

                    b.Property<DateTime>("AccessTokenExpiryUtc");

                    b.Property<string>("CreatedById");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("LastUpdated");

                    b.Property<string>("OwnerKey");

                    b.Property<string>("RefreshToken");

                    b.Property<string>("TokenType")
                        .IsRequired();

                    b.Property<string>("UpdatedById");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("OwnerKey")
                        .IsUnique();

                    b.HasIndex("UpdatedById");

                    b.ToTable("HouserooCredentials");
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
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("WCA.Domain.Models.ConveyancingSignupSubmission", b =>
                {
                    b.Property<int>("ConveyancingSignupSubmissionId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ABN");

                    b.Property<bool>("AcceptedTermsAndConditions");

                    b.Property<bool>("AcknowledgedFeesAndCharges");

                    b.Property<string>("Address1");

                    b.Property<string>("Address2");

                    b.Property<string>("City");

                    b.Property<string>("Company");

                    b.Property<string>("ConveyancingApp");

                    b.Property<string>("Email");

                    b.Property<string>("Firstname");

                    b.Property<string>("Lastname");

                    b.Property<string>("Phone");

                    b.Property<string>("Postcode");

                    b.Property<string>("State");

                    b.Property<DateTime>("SubmittedDateTimeUtc");

                    b.HasKey("ConveyancingSignupSubmissionId");

                    b.ToTable("ConveyancingSignupSubmissions");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("WCA.Domain.Models.Account.WCAUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("WCA.Domain.Models.Account.WCAUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("WCA.Domain.Models.Account.WCAUser")
                        .WithMany("Roles")
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

            modelBuilder.Entity("WCA.Domain.Actionstep.ActionstepOrg", b =>
                {
                    b.HasOne("WCA.Domain.Models.Account.WCAUser", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("WCA.Domain.Models.Account.WCAUser", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedById");
                });

            modelBuilder.Entity("WCA.Domain.Houseroo.HouserooCredential", b =>
                {
                    b.HasOne("WCA.Domain.Models.Account.WCAUser", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById");

                    b.HasOne("WCA.Domain.Models.Account.WCAUser", "Owner")
                        .WithOne()
                        .HasForeignKey("WCA.Domain.Houseroo.HouserooCredential", "OwnerKey");

                    b.HasOne("WCA.Domain.Models.Account.WCAUser", "UpdatedBy")
                        .WithMany()
                        .HasForeignKey("UpdatedById");
                });
        }
    }
}
