using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_PriceOffer_20250616_170600 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "Status",
            table: "PriceOffer",
            newName: "ApprovalStatus");

        migrationBuilder.AddColumn<string>(
            name: "SalesOutcomeStatus",
            table: "PriceOffer",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "BuyerTypeId",
            table: "PriceOffer",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("8B56E3FE-DC6D-4037-9C6D-7D5C66A607BD"));

        migrationBuilder.CreateIndex(
            name: "IX_PriceOffer_BuyerTypeId",
            table: "PriceOffer",
            column: "BuyerTypeId");

        migrationBuilder.AddForeignKey(
            name: "FK_PriceOffer_SystemCategories_BuyerTypeId",
            table: "PriceOffer",
            column: "BuyerTypeId",
            principalTable: "SystemCategories",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_PriceOffer_SystemCategories_BuyerTypeId",
            table: "PriceOffer");

        migrationBuilder.DropIndex(
            name: "IX_PriceOffer_BuyerTypeId",
            table: "PriceOffer");

        migrationBuilder.DropColumn(
            name: "ApprovalStatus",
            table: "PriceOffer");

        migrationBuilder.DropColumn(
            name: "BuyerTypeId",
            table: "PriceOffer");

        migrationBuilder.RenameColumn(
            name: "SalesOutcomeStatus",
            table: "PriceOffer",
            newName: "Status");
    }
}
