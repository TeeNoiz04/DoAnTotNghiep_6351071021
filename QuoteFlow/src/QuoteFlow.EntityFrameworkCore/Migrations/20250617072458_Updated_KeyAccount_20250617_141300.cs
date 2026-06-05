using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_KeyAccount_20250617_141300 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "KeyAccountId",
            table: "ApprovalRoute",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "KeyAccountId",
            table: "ApprovalHistories",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_KeyAccount_Evaluations_KeyAccount_Id",
            table: "KeyAccount_Evaluations",
            column: "KeyAccount_Id");

        migrationBuilder.CreateIndex(
            name: "IX_ApprovalRoute_KeyAccountId",
            table: "ApprovalRoute",
            column: "KeyAccountId");

        migrationBuilder.CreateIndex(
            name: "IX_ApprovalHistories_KeyAccountId",
            table: "ApprovalHistories",
            column: "KeyAccountId");

        migrationBuilder.AddForeignKey(
            name: "FK_ApprovalHistories_KeyAccount_KeyAccountId",
            table: "ApprovalHistories",
            column: "KeyAccountId",
            principalTable: "KeyAccount",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_ApprovalRoute_KeyAccount_KeyAccountId",
            table: "ApprovalRoute",
            column: "KeyAccountId",
            principalTable: "KeyAccount",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_KeyAccount_Evaluations_KeyAccount_KeyAccount_Id",
            table: "KeyAccount_Evaluations",
            column: "KeyAccount_Id",
            principalTable: "KeyAccount",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_ApprovalHistories_KeyAccount_KeyAccountId",
            table: "ApprovalHistories");

        migrationBuilder.DropForeignKey(
            name: "FK_ApprovalRoute_KeyAccount_KeyAccountId",
            table: "ApprovalRoute");

        migrationBuilder.DropForeignKey(
            name: "FK_KeyAccount_Evaluations_KeyAccount_KeyAccount_Id",
            table: "KeyAccount_Evaluations");

        migrationBuilder.DropIndex(
            name: "IX_KeyAccount_Evaluations_KeyAccount_Id",
            table: "KeyAccount_Evaluations");

        migrationBuilder.DropIndex(
            name: "IX_ApprovalRoute_KeyAccountId",
            table: "ApprovalRoute");

        migrationBuilder.DropIndex(
            name: "IX_ApprovalHistories_KeyAccountId",
            table: "ApprovalHistories");

        migrationBuilder.DropColumn(
            name: "KeyAccountId",
            table: "ApprovalRoute");

        migrationBuilder.DropColumn(
            name: "KeyAccountId",
            table: "ApprovalHistories");
    }
}
