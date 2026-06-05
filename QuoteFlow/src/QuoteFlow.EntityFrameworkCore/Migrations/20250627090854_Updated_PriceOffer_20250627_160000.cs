using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_PriceOffer_20250627_160000 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "PriceOfferId",
            table: "Attachments",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_Attachments_PriceOfferId",
            table: "Attachments",
            column: "PriceOfferId");

        migrationBuilder.AddForeignKey(
            name: "FK_Attachments_PriceOffer_PriceOfferId",
            table: "Attachments",
            column: "PriceOfferId",
            principalTable: "PriceOffer",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Attachments_PriceOffer_PriceOfferId",
            table: "Attachments");

        migrationBuilder.DropIndex(
            name: "IX_Attachments_PriceOfferId",
            table: "Attachments");

        migrationBuilder.DropColumn(
            name: "PriceOfferId",
            table: "Attachments");
    }
}
