using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Flaw.Migrations
{
    public partial class abc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActivePrivilegeNo",
                table: "MembershipFees");

            migrationBuilder.AddColumn<int>(
                name: "LicenseNumber",
                table: "MembershipFees",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LicenseNumber",
                table: "MembershipFees");

            migrationBuilder.AddColumn<int>(
                name: "ActivePrivilegeNo",
                table: "MembershipFees",
                nullable: false,
                defaultValue: 0);
        }
    }
}
