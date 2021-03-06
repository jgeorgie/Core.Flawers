﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Flaw.Data;

namespace Flaw.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170115115526_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Flaw.Controllers.MyIdentityRole", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Flaw.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id");

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FullName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedUserName")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Flaw.Models.CashModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Account");

                    b.Property<string>("AccountingPass");

                    b.Property<double>("Amount");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Destination");

                    b.Property<string>("FullName");

                    b.Property<int>("MembershipFeeId");

                    b.Property<string>("OrdersNumber");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("MembershipFeeId");

                    b.ToTable("CashModel");
                });

            modelBuilder.Entity("Flaw.Models.FeeAmountChangeModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("ChangeDate");

                    b.Property<int?>("FeeId");

                    b.Property<int>("MembershipFeeForeignKey");

                    b.Property<double>("NewAmount");

                    b.Property<double>("OldAmount");

                    b.HasKey("Id");

                    b.HasIndex("FeeId");

                    b.ToTable("FeeAmountChangeModels");
                });

            modelBuilder.Entity("Flaw.Models.FeeStateChangeModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("ChangeDate");

                    b.Property<int>("MembershipFeeForeignKey");

                    b.Property<int>("NewState");

                    b.Property<int>("PreviousState");

                    b.HasKey("Id");

                    b.HasIndex("MembershipFeeForeignKey");

                    b.ToTable("FeeStateChanges");
                });

            modelBuilder.Entity("Flaw.Models.MembershipFee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("ActivePrivilegeEnd");

                    b.Property<DateTime?>("ActivePrivilegeStart");

                    b.Property<double>("AmountWithDiscount");

                    b.Property<int>("CurrentState");

                    b.Property<double?>("Deposit");

                    b.Property<DateTime>("End");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<double>("LeftOver");

                    b.Property<int>("LicenseNumber");

                    b.Property<string>("MiddleName")
                        .IsRequired();

                    b.Property<double>("MonthlyPay");

                    b.Property<DateTime?>("Paused");

                    b.Property<int>("Periodicity");

                    b.Property<int?>("PrivilegeId");

                    b.Property<string>("PrivilegeType");

                    b.Property<DateTime?>("Reactiveted");

                    b.Property<double>("RealAmount");

                    b.Property<DateTime>("Start");

                    b.Property<int?>("TotalDaysPaused");

                    b.Property<double>("currentDebt");

                    b.HasKey("Id");

                    b.HasIndex("PrivilegeId");

                    b.ToTable("MembershipFees");
                });

            modelBuilder.Entity("Flaw.Models.PendingPaymentModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Amount");

                    b.Property<int>("CashPaymentForeignKey");

                    b.Property<int?>("CashPaymentId");

                    b.Property<double?>("DepositOrDebt");

                    b.Property<int?>("FeeId");

                    b.Property<int>("MembershipFeeForeignKey");

                    b.Property<DateTime>("PayedOn");

                    b.Property<DateTime>("PaymentDeadline");

                    b.Property<int>("Status");

                    b.Property<int>("TransferPaymentForeignKey");

                    b.Property<int?>("TransferPaymentId");

                    b.HasKey("Id");

                    b.HasIndex("CashPaymentId");

                    b.HasIndex("FeeId");

                    b.HasIndex("TransferPaymentId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("Flaw.Models.Privilege", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<int>("Discount");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.ToTable("Privileges");
                });

            modelBuilder.Entity("Flaw.Models.PrivilegeModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("End");

                    b.Property<int>("MembershipFeeFoeignKey");

                    b.Property<long>("PrivilegeNumber");

                    b.Property<DateTime>("Start");

                    b.Property<string>("Type");

                    b.HasKey("Id");

                    b.HasIndex("MembershipFeeFoeignKey");

                    b.ToTable("PrivilegeModels");
                });

            modelBuilder.Entity("Flaw.Models.TransferPayment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Amount");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Destination");

                    b.Property<string>("FullName");

                    b.Property<int>("MembershipFeeId");

                    b.Property<string>("PaymentNo");

                    b.HasKey("Id");

                    b.HasIndex("MembershipFeeId");

                    b.ToTable("TransferPayments");
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

                    b.HasIndex("UserId");

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

            modelBuilder.Entity("Flaw.Models.CashModel", b =>
                {
                    b.HasOne("Flaw.Models.MembershipFee", "Fee")
                        .WithMany("CashPayments")
                        .HasForeignKey("MembershipFeeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Flaw.Models.FeeAmountChangeModel", b =>
                {
                    b.HasOne("Flaw.Models.MembershipFee", "Fee")
                        .WithMany("AmountChanges")
                        .HasForeignKey("FeeId");
                });

            modelBuilder.Entity("Flaw.Models.FeeStateChangeModel", b =>
                {
                    b.HasOne("Flaw.Models.MembershipFee", "Fee")
                        .WithMany("FeeStateChanges")
                        .HasForeignKey("MembershipFeeForeignKey")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Flaw.Models.MembershipFee", b =>
                {
                    b.HasOne("Flaw.Models.Privilege")
                        .WithMany("Fees")
                        .HasForeignKey("PrivilegeId");
                });

            modelBuilder.Entity("Flaw.Models.PendingPaymentModel", b =>
                {
                    b.HasOne("Flaw.Models.CashModel", "CashPayment")
                        .WithMany()
                        .HasForeignKey("CashPaymentId");

                    b.HasOne("Flaw.Models.MembershipFee", "Fee")
                        .WithMany()
                        .HasForeignKey("FeeId");

                    b.HasOne("Flaw.Models.TransferPayment", "TransferPayment")
                        .WithMany()
                        .HasForeignKey("TransferPaymentId");
                });

            modelBuilder.Entity("Flaw.Models.PrivilegeModel", b =>
                {
                    b.HasOne("Flaw.Models.MembershipFee", "Fee")
                        .WithMany("PrivilegeModels")
                        .HasForeignKey("MembershipFeeFoeignKey")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Flaw.Models.TransferPayment", b =>
                {
                    b.HasOne("Flaw.Models.MembershipFee", "Fee")
                        .WithMany("TransferPayments")
                        .HasForeignKey("MembershipFeeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Flaw.Controllers.MyIdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Flaw.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Flaw.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Flaw.Controllers.MyIdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Flaw.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
