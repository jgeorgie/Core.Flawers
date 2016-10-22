using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Flaw.Migrations
{
    public partial class FeeStateChangeModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FeeStateChanges",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ChangeDate = table.Column<DateTime>(nullable: false),
                    MembershipFeeForeignKey = table.Column<string>(nullable: true),
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

            migrationBuilder.CreateIndex(
                name: "IX_FeeStateChanges_MembershipFeeForeignKey",
                table: "FeeStateChanges",
                column: "MembershipFeeForeignKey");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeeStateChanges");
        }
    }
}
