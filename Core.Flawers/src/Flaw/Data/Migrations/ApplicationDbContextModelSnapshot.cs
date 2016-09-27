using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Flaw.Data;

namespace Flaw.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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
                    b.Property<string>("Id");

                    b.Property<string>("AccountingPass");

                    b.Property<double>("Amount");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Destination");

                    b.Property<string>("FullName");

                    b.Property<string>("OrdersNumber");

                    b.Property<string>("PaymentModelId");

                    b.Property<int>("Type");

                    b.Property<string>("Аccount");

                    b.HasKey("Id");

                    b.HasIndex("PaymentModelId")
                        .IsUnique();

                    b.ToTable("CashModel");
                });

            modelBuilder.Entity("Flaw.Models.FeeAmountChangeModel", b =>
                {
                    b.Property<string>("id");

                    b.Property<DateTime>("ChangeDate");

                    b.Property<string>("FeeId");

                    b.Property<string>("MembershipFeeForeignKey");

                    b.Property<double>("NewAmount");

                    b.Property<double>("OldAmount");

                    b.HasKey("id");

                    b.HasIndex("FeeId");

                    b.ToTable("FeeAmountChangeModels");
                });

            modelBuilder.Entity("Flaw.Models.MembershipFee", b =>
                {
                    b.Property<string>("Id");

                    b.Property<double>("AmountWithDiscount");

                    b.Property<int>("CurrentState");

                    b.Property<DateTime>("End");

                    b.Property<string>("FullName");

                    b.Property<double>("LeftOver");

                    b.Property<DateTime?>("Paused");

                    b.Property<int>("Periodicity");

                    b.Property<DateTime?>("Reactiveted");

                    b.Property<double>("RealAmount");

                    b.Property<DateTime>("Start");

                    b.Property<int?>("TotalDaysPaused");

                    b.Property<double>("currentDebt");

                    b.HasKey("Id");

                    b.ToTable("MembershipFees");
                });

            modelBuilder.Entity("Flaw.Models.PaymentModel", b =>
                {
                    b.Property<string>("Id");

                    b.Property<double>("Amount");

                    b.Property<string>("CashPaymentId");

                    b.Property<DateTime>("Date");

                    b.Property<string>("MembershipFeeForeignKey");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("MembershipFeeForeignKey");

                    b.ToTable("PaymentModel");
                });

            modelBuilder.Entity("Flaw.Models.Privilege", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("Description");

                    b.Property<int>("Discount");

                    b.Property<DateTime>("End");

                    b.Property<string>("MembershipFeeForeignKey");

                    b.Property<DateTime>("Start");

                    b.HasKey("Id");

                    b.HasIndex("MembershipFeeForeignKey")
                        .IsUnique();

                    b.ToTable("Privileges");
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
                    b.HasOne("Flaw.Models.PaymentModel", "Payment")
                        .WithOne("CashPayment")
                        .HasForeignKey("Flaw.Models.CashModel", "PaymentModelId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Flaw.Models.FeeAmountChangeModel", b =>
                {
                    b.HasOne("Flaw.Models.MembershipFee", "Fee")
                        .WithMany()
                        .HasForeignKey("FeeId");
                });

            modelBuilder.Entity("Flaw.Models.PaymentModel", b =>
                {
                    b.HasOne("Flaw.Models.MembershipFee", "Fee")
                        .WithMany("Payments")
                        .HasForeignKey("MembershipFeeForeignKey")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Flaw.Models.Privilege", b =>
                {
                    b.HasOne("Flaw.Models.MembershipFee", "Fee")
                        .WithOne("Privilege")
                        .HasForeignKey("Flaw.Models.Privilege", "MembershipFeeForeignKey")
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
