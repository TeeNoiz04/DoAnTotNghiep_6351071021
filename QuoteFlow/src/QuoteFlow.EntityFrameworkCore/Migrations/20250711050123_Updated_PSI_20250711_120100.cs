using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_PSI_20250711_120100 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "PSIId",
            table: "ApprovalRoute",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "PSI_Id",
            table: "ApprovalRoute",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "PSIId",
            table: "ApprovalHistories",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "PSI_Id",
            table: "ApprovalHistories",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_ApprovalRoute_PSIId",
            table: "ApprovalRoute",
            column: "PSIId");

        migrationBuilder.CreateIndex(
            name: "IX_ApprovalHistories_PSIId",
            table: "ApprovalHistories",
            column: "PSIId");

        migrationBuilder.AddForeignKey(
            name: "FK_ApprovalHistories_PSI_PSIId",
            table: "ApprovalHistories",
            column: "PSIId",
            principalTable: "PSI",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_ApprovalRoute_PSI_PSIId",
            table: "ApprovalRoute",
            column: "PSIId",
            principalTable: "PSI",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_ApprovalHistories_PSI_PSIId",
            table: "ApprovalHistories");

        migrationBuilder.DropForeignKey(
            name: "FK_ApprovalRoute_PSI_PSIId",
            table: "ApprovalRoute");

        migrationBuilder.DropIndex(
            name: "IX_ApprovalRoute_PSIId",
            table: "ApprovalRoute");

        migrationBuilder.DropIndex(
            name: "IX_ApprovalHistories_PSIId",
            table: "ApprovalHistories");

        migrationBuilder.DropColumn(
            name: "PSIId",
            table: "ApprovalRoute");

        migrationBuilder.DropColumn(
            name: "PSI_Id",
            table: "ApprovalRoute");

        migrationBuilder.DropColumn(
            name: "PSIId",
            table: "ApprovalHistories");

        migrationBuilder.DropColumn(
            name: "PSI_Id",
            table: "ApprovalHistories");
    }
}
