using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Flaw.Models;

namespace Flaw.Migrations
{
    public partial class all : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Periodicity",
                table: "MembershipFees",
                nullable: false,
                defaultValue: FeePeriodicity.Month);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Periodicity",
                table: "MembershipFees");
        }
    }
}
