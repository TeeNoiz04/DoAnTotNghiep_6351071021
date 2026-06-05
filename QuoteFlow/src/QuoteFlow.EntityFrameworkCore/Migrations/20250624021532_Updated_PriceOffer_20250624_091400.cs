using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_PriceOffer_20250624_091400 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<DateTime>(
            name: "SpecialInputPriceAppliedTime",
            table: "PriceOffer",
            type: "datetime2",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "SpecialInputPriceAssignerFullName",
            table: "PriceOffer",
            type: "nvarchar(255)",
            maxLength: 255,
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "SpecialInputPriceAssignerId",
            table: "PriceOffer",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "SpecialInputPriceAssignerUsername",
            table: "PriceOffer",
            type: "nvarchar(100)",
            maxLength: 100,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "SpecialInputPriceAssignmentNote",
            table: "PriceOffer",
            type: "nvarchar(4000)",
            maxLength: 4000,
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "SpecialInputPriceId",
            table: "PriceOffer",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_PriceOffer_SpecialInputPriceId",
            table: "PriceOffer",
            column: "SpecialInputPriceId");

        migrationBuilder.AddForeignKey(
            name: "FK_PriceOffer_SpecialInputPrice_SpecialInputPriceId",
            table: "PriceOffer",
            column: "SpecialInputPriceId",
            principalTable: "SpecialInputPrice",
            principalColumn: "Id",
            onDelete: ReferentialAction.SetNull);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_PriceOffer_SpecialInputPrice_SpecialInputPriceId",
            table: "PriceOffer");

        migrationBuilder.DropIndex(
            name: "IX_PriceOffer_SpecialInputPriceId",
            table: "PriceOffer");

        migrationBuilder.DropColumn(
            name: "SpecialInputPriceAppliedTime",
            table: "PriceOffer");

        migrationBuilder.DropColumn(
            name: "SpecialInputPriceAssignerFullName",
            table: "PriceOffer");

        migrationBuilder.DropColumn(
            name: "SpecialInputPriceAssignerId",
            table: "PriceOffer");

        migrationBuilder.DropColumn(
            name: "SpecialInputPriceAssignerUsername",
            table: "PriceOffer");

        migrationBuilder.DropColumn(
            name: "SpecialInputPriceAssignmentNote",
            table: "PriceOffer");

        migrationBuilder.DropColumn(
            name: "SpecialInputPriceId",
            table: "PriceOffer");
    }
}
