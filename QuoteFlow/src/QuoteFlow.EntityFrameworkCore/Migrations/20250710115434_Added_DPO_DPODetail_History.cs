using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Added_DPO_DPODetail_History : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "DPODetailId",
            table: "ApprovalHistories",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "DPOId",
            table: "ApprovalHistories",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_ApprovalHistories_DPODetailId",
            table: "ApprovalHistories",
            column: "DPODetailId");

        migrationBuilder.CreateIndex(
            name: "IX_ApprovalHistories_DPOId",
            table: "ApprovalHistories",
            column: "DPOId");

        migrationBuilder.AddForeignKey(
            name: "FK_ApprovalHistories_DPODetail_DPODetailId",
            table: "ApprovalHistories",
            column: "DPODetailId",
            principalTable: "DPODetail",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);

        migrationBuilder.AddForeignKey(
            name: "FK_ApprovalHistories_DPO_DPOId",
            table: "ApprovalHistories",
            column: "DPOId",
            principalTable: "DPO",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_ApprovalHistories_DPODetail_DPODetailId",
            table: "ApprovalHistories");

        migrationBuilder.DropForeignKey(
            name: "FK_ApprovalHistories_DPO_DPOId",
            table: "ApprovalHistories");

        migrationBuilder.DropIndex(
            name: "IX_ApprovalHistories_DPODetailId",
            table: "ApprovalHistories");

        migrationBuilder.DropIndex(
            name: "IX_ApprovalHistories_DPOId",
            table: "ApprovalHistories");

        migrationBuilder.DropColumn(
            name: "DPODetailId",
            table: "ApprovalHistories");

        migrationBuilder.DropColumn(
            name: "DPOId",
            table: "ApprovalHistories");
    }
}
