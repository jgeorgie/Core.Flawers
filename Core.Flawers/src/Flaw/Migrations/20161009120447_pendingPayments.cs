using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Flaw.Migrations
{
    public partial class pendingPayments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Amount = table.Column<double>(nullable: false),
                    CashPaymentForeignKey = table.Column<string>(nullable: true),
                    CashPaymentId = table.Column<string>(nullable: true),
                    FeeId = table.Column<string>(nullable: true),
                    MembershipFeeForeignKey = table.Column<string>(nullable: true),
                    PaymentDeadline = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    TransferPaymentForeignKey = table.Column<string>(nullable: true),
                    TransferPaymentId = table.Column<string>(nullable: true)
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");
        }
    }
}
