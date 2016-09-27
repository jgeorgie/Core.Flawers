using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Flaw.Migrations
{
    public partial class feeAmountChangeModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FeeAmountChangeModels",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    ChangeDate = table.Column<DateTime>(nullable: false),
                    FeeId = table.Column<string>(nullable: true),
                    MembershipFeeForeignKey = table.Column<string>(nullable: true),
                    NewAmount = table.Column<double>(nullable: false),
                    OldAmount = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeAmountChangeModels", x => x.id);
                    table.ForeignKey(
                        name: "FK_FeeAmountChangeModels_MembershipFees_FeeId",
                        column: x => x.FeeId,
                        principalTable: "MembershipFees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeeAmountChangeModels_FeeId",
                table: "FeeAmountChangeModels",
                column: "FeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeeAmountChangeModels");
        }
    }
}
