using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Flaw.Migrations
{
    public partial class mig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CashModel_Payments_PaymentModelId",
                table: "CashModel");

            migrationBuilder.DropIndex(
                name: "IX_CashModel_PaymentModelId",
                table: "CashModel");

            migrationBuilder.DropColumn(
                name: "PaymentModelId",
                table: "CashModel");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "PendingPayments");

            migrationBuilder.CreateTable(
                name: "TransferPayments",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Destination = table.Column<string>(nullable: true),
                    MembershipFeeId = table.Column<string>(nullable: true)
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

            migrationBuilder.AddColumn<string>(
                name: "MembershipFeeId",
                table: "CashModel",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CashModel_MembershipFeeId",
                table: "CashModel",
                column: "MembershipFeeId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferPayments_MembershipFeeId",
                table: "TransferPayments",
                column: "MembershipFeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_CashModel_MembershipFees_MembershipFeeId",
                table: "CashModel",
                column: "MembershipFeeId",
                principalTable: "MembershipFees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CashModel_MembershipFees_MembershipFeeId",
                table: "CashModel");

            migrationBuilder.DropIndex(
                name: "IX_CashModel_MembershipFeeId",
                table: "CashModel");

            migrationBuilder.DropColumn(
                name: "MembershipFeeId",
                table: "CashModel");

            migrationBuilder.DropTable(
                name: "TransferPayments");

            migrationBuilder.CreateTable(
                name: "PendingPayments",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    MembershipFeeForeignKey = table.Column<string>(nullable: true),
                    PaymentDeadline = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PendingPayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PendingPayments_MembershipFees_MembershipFeeForeignKey",
                        column: x => x.MembershipFeeForeignKey,
                        principalTable: "MembershipFees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    CashPaymentId = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    FeeId = table.Column<string>(nullable: true),
                    MembershipFeeForeignKey = table.Column<string>(nullable: true),
                    PendingPaymentModelId = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_MembershipFees_FeeId",
                        column: x => x.FeeId,
                        principalTable: "MembershipFees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_PendingPayments_PendingPaymentModelId",
                        column: x => x.PendingPaymentModelId,
                        principalTable: "PendingPayments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.AddColumn<string>(
                name: "PaymentModelId",
                table: "CashModel",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CashModel_PaymentModelId",
                table: "CashModel",
                column: "PaymentModelId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_FeeId",
                table: "Payments",
                column: "FeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PendingPaymentModelId",
                table: "Payments",
                column: "PendingPaymentModelId");

            migrationBuilder.CreateIndex(
                name: "IX_PendingPayments_MembershipFeeForeignKey",
                table: "PendingPayments",
                column: "MembershipFeeForeignKey");

            migrationBuilder.AddForeignKey(
                name: "FK_CashModel_Payments_PaymentModelId",
                table: "CashModel",
                column: "PaymentModelId",
                principalTable: "Payments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
