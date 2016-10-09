using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Flaw.Migrations
{
    public partial class modelChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Privileges_MembershipFeeForeignKey",
                table: "Privileges");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "MembershipFees");

            migrationBuilder.AddColumn<int>(
                name: "PrivilegeNumber",
                table: "Privileges",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Privileges",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActivePrivilegeNo",
                table: "MembershipFees",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Deposit",
                table: "MembershipFees",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "MembershipFees",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "MembershipFees",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "MembershipFees",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Privileges_MembershipFeeForeignKey",
                table: "Privileges",
                column: "MembershipFeeForeignKey");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Privileges_MembershipFeeForeignKey",
                table: "Privileges");

            migrationBuilder.DropColumn(
                name: "PrivilegeNumber",
                table: "Privileges");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Privileges");

            migrationBuilder.DropColumn(
                name: "ActivePrivilegeNo",
                table: "MembershipFees");

            migrationBuilder.DropColumn(
                name: "Deposit",
                table: "MembershipFees");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "MembershipFees");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "MembershipFees");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "MembershipFees");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "MembershipFees",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Privileges_MembershipFeeForeignKey",
                table: "Privileges",
                column: "MembershipFeeForeignKey",
                unique: true);
        }
    }
}
