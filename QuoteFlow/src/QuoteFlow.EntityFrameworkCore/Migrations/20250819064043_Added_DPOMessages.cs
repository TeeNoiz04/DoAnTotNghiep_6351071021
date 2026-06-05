using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Added_DPOMessages : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "FilName",
            table: "MaterialStockUpload",
            newName: "FileName");

        migrationBuilder.AddColumn<Guid>(
            name: "DPOId",
            table: "Discussion",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_Discussion_DPOId",
            table: "Discussion",
            column: "DPOId");

        migrationBuilder.AddForeignKey(
            name: "FK_Discussion_DPO_DPOId",
            table: "Discussion",
            column: "DPOId",
            principalTable: "DPO",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Discussion_DPO_DPOId",
            table: "Discussion");

        migrationBuilder.DropIndex(
            name: "IX_Discussion_DPOId",
            table: "Discussion");

        migrationBuilder.DropColumn(
            name: "DPOId",
            table: "Discussion");

        migrationBuilder.RenameColumn(
            name: "FileName",
            table: "MaterialStockUpload",
            newName: "FilName");
    }
}
