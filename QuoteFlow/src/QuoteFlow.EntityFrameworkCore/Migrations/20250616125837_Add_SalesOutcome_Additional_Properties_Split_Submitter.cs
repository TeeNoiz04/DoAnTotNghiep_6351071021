using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Add_SalesOutcome_Additional_Properties_Split_Submitter : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<Guid>(
            name: "DeleterId",
            table: "PriceOffer_Customer",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "DeleterName",
            table: "PriceOffer_Customer",
            type: "nvarchar(255)",
            maxLength: 255,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "DeleterUsername",
            table: "PriceOffer_Customer",
            type: "nvarchar(100)",
            maxLength: 100,
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "DeletionTime",
            table: "PriceOffer_Customer",
            type: "datetime2",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "IsDeleted",
            table: "PriceOffer_Customer",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<DateTime>(
            name: "OutcomeSubmittedAt",
            table: "PriceOffer",
            type: "datetime2",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "OutcomeSubmitterFullName",
            table: "PriceOffer",
            type: "nvarchar(255)",
            maxLength: 255,
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "OutcomeSubmitterId",
            table: "PriceOffer",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "OutcomeSubmitterUsername",
            table: "PriceOffer",
            type: "nvarchar(100)",
            maxLength: 100,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "SalesOutcomeNote",
            table: "PriceOffer",
            type: "nvarchar(4000)",
            maxLength: 4000,
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "DeleterId",
            table: "PriceOffer_Customer");

        migrationBuilder.DropColumn(
            name: "DeleterName",
            table: "PriceOffer_Customer");

        migrationBuilder.DropColumn(
            name: "DeleterUsername",
            table: "PriceOffer_Customer");

        migrationBuilder.DropColumn(
            name: "DeletionTime",
            table: "PriceOffer_Customer");

        migrationBuilder.DropColumn(
            name: "IsDeleted",
            table: "PriceOffer_Customer");

        migrationBuilder.DropColumn(
            name: "OutcomeSubmittedAt",
            table: "PriceOffer");

        migrationBuilder.DropColumn(
            name: "OutcomeSubmitterFullName",
            table: "PriceOffer");

        migrationBuilder.DropColumn(
            name: "OutcomeSubmitterId",
            table: "PriceOffer");

        migrationBuilder.DropColumn(
            name: "OutcomeSubmitterUsername",
            table: "PriceOffer");

        migrationBuilder.DropColumn(
            name: "SalesOutcomeNote",
            table: "PriceOffer");
    }
}
