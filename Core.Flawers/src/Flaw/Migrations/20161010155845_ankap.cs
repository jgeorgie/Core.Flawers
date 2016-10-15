using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Flaw.Migrations
{
    public partial class ankap : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Privileges_MembershipFees_MembershipFeeForeignKey",
                table: "Privileges");

            migrationBuilder.DropIndex(
                name: "IX_Privileges_MembershipFeeForeignKey",
                table: "Privileges");

            migrationBuilder.DropColumn(
                name: "End",
                table: "Privileges");

            migrationBuilder.DropColumn(
                name: "MembershipFeeForeignKey",
                table: "Privileges");

            migrationBuilder.DropColumn(
                name: "Start",
                table: "Privileges");

            migrationBuilder.AddColumn<DateTime>(
                name: "ActivePrivilegeEnd",
                table: "MembershipFees",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ActivePrivilegeStart",
                table: "MembershipFees",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrivilegeId",
                table: "MembershipFees",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrivilegeType",
                table: "MembershipFees",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MembershipFees_PrivilegeId",
                table: "MembershipFees",
                column: "PrivilegeId");

            migrationBuilder.AddForeignKey(
                name: "FK_MembershipFees_Privileges_PrivilegeId",
                table: "MembershipFees",
                column: "PrivilegeId",
                principalTable: "Privileges",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MembershipFees_Privileges_PrivilegeId",
                table: "MembershipFees");

            migrationBuilder.DropIndex(
                name: "IX_MembershipFees_PrivilegeId",
                table: "MembershipFees");

            migrationBuilder.DropColumn(
                name: "ActivePrivilegeEnd",
                table: "MembershipFees");

            migrationBuilder.DropColumn(
                name: "ActivePrivilegeStart",
                table: "MembershipFees");

            migrationBuilder.DropColumn(
                name: "PrivilegeId",
                table: "MembershipFees");

            migrationBuilder.DropColumn(
                name: "PrivilegeType",
                table: "MembershipFees");

            migrationBuilder.AddColumn<DateTime>(
                name: "End",
                table: "Privileges",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "MembershipFeeForeignKey",
                table: "Privileges",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Start",
                table: "Privileges",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Privileges_MembershipFeeForeignKey",
                table: "Privileges",
                column: "MembershipFeeForeignKey");

            migrationBuilder.AddForeignKey(
                name: "FK_Privileges_MembershipFees_MembershipFeeForeignKey",
                table: "Privileges",
                column: "MembershipFeeForeignKey",
                principalTable: "MembershipFees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
