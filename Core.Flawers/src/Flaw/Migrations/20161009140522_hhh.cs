using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Flaw.Migrations
{
    public partial class hhh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaymentNo",
                table: "TransferPayments",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DepositOrDebt",
                table: "Payments",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PayedOn",
                table: "Payments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentNo",
                table: "TransferPayments");

            migrationBuilder.DropColumn(
                name: "DepositOrDebt",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PayedOn",
                table: "Payments");
        }
    }
}
