using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Flaw.Migrations
{
    public partial class fgvuasyfgvbsa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Аccount",
                table: "CashModel");

            migrationBuilder.AddColumn<string>(
                name: "Account",
                table: "CashModel",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Account",
                table: "CashModel");

            migrationBuilder.AddColumn<string>(
                name: "Аccount",
                table: "CashModel",
                nullable: true);
        }
    }
}
