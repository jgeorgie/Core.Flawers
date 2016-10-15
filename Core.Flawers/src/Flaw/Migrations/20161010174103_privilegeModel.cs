using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Flaw.Migrations
{
    public partial class privilegeModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrivilegeNumber",
                table: "Privileges");

            migrationBuilder.CreateTable(
                name: "PrivilegeModels",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    End = table.Column<DateTime>(nullable: false),
                    MembershipFeeFoeignKey = table.Column<string>(nullable: true),
                    PrivilegeNumber = table.Column<int>(nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_PrivilegeModels_MembershipFeeFoeignKey",
                table: "PrivilegeModels",
                column: "MembershipFeeFoeignKey");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrivilegeModels");

            migrationBuilder.AddColumn<int>(
                name: "PrivilegeNumber",
                table: "Privileges",
                nullable: false,
                defaultValue: 0);
        }
    }
}
