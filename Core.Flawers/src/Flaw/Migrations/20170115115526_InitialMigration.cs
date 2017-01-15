using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Flaw.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    FullName = table.Column<string>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    SecurityStamp = table.Column<string>(nullable: true),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Privileges",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    Discount = table.Column<int>(nullable: false),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Privileges", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MembershipFees",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ActivePrivilegeEnd = table.Column<DateTime>(nullable: true),
                    ActivePrivilegeStart = table.Column<DateTime>(nullable: true),
                    AmountWithDiscount = table.Column<double>(nullable: false),
                    CurrentState = table.Column<int>(nullable: false),
                    Deposit = table.Column<double>(nullable: true),
                    End = table.Column<DateTime>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    LeftOver = table.Column<double>(nullable: false),
                    LicenseNumber = table.Column<int>(nullable: false),
                    MiddleName = table.Column<string>(nullable: false),
                    MonthlyPay = table.Column<double>(nullable: false),
                    Paused = table.Column<DateTime>(nullable: true),
                    Periodicity = table.Column<int>(nullable: false),
                    PrivilegeId = table.Column<int>(nullable: true),
                    PrivilegeType = table.Column<string>(nullable: true),
                    Reactiveted = table.Column<DateTime>(nullable: true),
                    RealAmount = table.Column<double>(nullable: false),
                    Start = table.Column<DateTime>(nullable: false),
                    TotalDaysPaused = table.Column<int>(nullable: true),
                    currentDebt = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipFees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MembershipFees_Privileges_PrivilegeId",
                        column: x => x.PrivilegeId,
                        principalTable: "Privileges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CashModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Account = table.Column<string>(nullable: true),
                    AccountingPass = table.Column<string>(nullable: true),
                    Amount = table.Column<double>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Destination = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    MembershipFeeId = table.Column<int>(nullable: false),
                    OrdersNumber = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CashModel_MembershipFees_MembershipFeeId",
                        column: x => x.MembershipFeeId,
                        principalTable: "MembershipFees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeeAmountChangeModels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChangeDate = table.Column<DateTime>(nullable: false),
                    FeeId = table.Column<int>(nullable: true),
                    MembershipFeeForeignKey = table.Column<int>(nullable: false),
                    NewAmount = table.Column<double>(nullable: false),
                    OldAmount = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeAmountChangeModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeeAmountChangeModels_MembershipFees_FeeId",
                        column: x => x.FeeId,
                        principalTable: "MembershipFees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FeeStateChanges",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChangeDate = table.Column<DateTime>(nullable: false),
                    MembershipFeeForeignKey = table.Column<int>(nullable: false),
                    NewState = table.Column<int>(nullable: false),
                    PreviousState = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeStateChanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeeStateChanges_MembershipFees_MembershipFeeForeignKey",
                        column: x => x.MembershipFeeForeignKey,
                        principalTable: "MembershipFees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PrivilegeModels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    End = table.Column<DateTime>(nullable: false),
                    MembershipFeeFoeignKey = table.Column<int>(nullable: false),
                    PrivilegeNumber = table.Column<long>(nullable: false),
                    Start = table.Column<DateTime>(nullable: false),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivilegeModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrivilegeModels_MembershipFees_MembershipFeeFoeignKey",
                        column: x => x.MembershipFeeFoeignKey,
                        principalTable: "MembershipFees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransferPayments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<double>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Destination = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    MembershipFeeId = table.Column<int>(nullable: false),
                    PaymentNo = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransferPayments_MembershipFees_MembershipFeeId",
                        column: x => x.MembershipFeeId,
                        principalTable: "MembershipFees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<double>(nullable: false),
                    CashPaymentForeignKey = table.Column<int>(nullable: false),
                    CashPaymentId = table.Column<int>(nullable: true),
                    DepositOrDebt = table.Column<double>(nullable: true),
                    FeeId = table.Column<int>(nullable: true),
                    MembershipFeeForeignKey = table.Column<int>(nullable: false),
                    PayedOn = table.Column<DateTime>(nullable: false),
                    PaymentDeadline = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    TransferPaymentForeignKey = table.Column<int>(nullable: false),
                    TransferPaymentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_CashModel_CashPaymentId",
                        column: x => x.CashPaymentId,
                        principalTable: "CashModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_MembershipFees_FeeId",
                        column: x => x.FeeId,
                        principalTable: "MembershipFees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_TransferPayments_TransferPaymentId",
                        column: x => x.TransferPaymentId,
                        principalTable: "TransferPayments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CashModel_MembershipFeeId",
                table: "CashModel",
                column: "MembershipFeeId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeAmountChangeModels_FeeId",
                table: "FeeAmountChangeModels",
                column: "FeeId");

            migrationBuilder.CreateIndex(
                name: "IX_FeeStateChanges_MembershipFeeForeignKey",
                table: "FeeStateChanges",
                column: "MembershipFeeForeignKey");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipFees_PrivilegeId",
                table: "MembershipFees",
                column: "PrivilegeId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CashPaymentId",
                table: "Payments",
                column: "CashPaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_FeeId",
                table: "Payments",
                column: "FeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_TransferPaymentId",
                table: "Payments",
                column: "TransferPaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivilegeModels_MembershipFeeFoeignKey",
                table: "PrivilegeModels",
                column: "MembershipFeeFoeignKey");

            migrationBuilder.CreateIndex(
                name: "IX_TransferPayments_MembershipFeeId",
                table: "TransferPayments",
                column: "MembershipFeeId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeeAmountChangeModels");

            migrationBuilder.DropTable(
                name: "FeeStateChanges");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "PrivilegeModels");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CashModel");

            migrationBuilder.DropTable(
                name: "TransferPayments");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "MembershipFees");

            migrationBuilder.DropTable(
                name: "Privileges");
        }
    }
}
