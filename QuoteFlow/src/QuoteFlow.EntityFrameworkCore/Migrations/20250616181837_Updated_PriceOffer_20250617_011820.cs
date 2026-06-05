using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_PriceOffer_20250617_011820 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "SalesOutcomeStatus",
            table: "PriceOffer",
            newName: "ProjectResultStatus");

        migrationBuilder.RenameColumn(
            name: "SalesOutcomeNote",
            table: "PriceOffer",
            newName: "ProjectResultNote");

        migrationBuilder.RenameColumn(
            name: "OutcomeSubmitterUsername",
            table: "PriceOffer",
            newName: "ProjectResultSubmitterUsername");

        migrationBuilder.RenameColumn(
            name: "OutcomeSubmitterId",
            table: "PriceOffer",
            newName: "ProjectResultSubmitterId");

        migrationBuilder.RenameColumn(
            name: "OutcomeSubmitterFullName",
            table: "PriceOffer",
            newName: "ProjectResultSubmitterFullName");

        migrationBuilder.RenameColumn(
            name: "OutcomeSubmittedAt",
            table: "PriceOffer",
            newName: "ProjectResultSubmittedAt");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "ProjectResultSubmitterUsername",
            table: "PriceOffer",
            newName: "OutcomeSubmitterUsername");

        migrationBuilder.RenameColumn(
            name: "ProjectResultSubmitterId",
            table: "PriceOffer",
            newName: "OutcomeSubmitterId");

        migrationBuilder.RenameColumn(
            name: "ProjectResultSubmitterFullName",
            table: "PriceOffer",
            newName: "OutcomeSubmitterFullName");

        migrationBuilder.RenameColumn(
            name: "ProjectResultSubmittedAt",
            table: "PriceOffer",
            newName: "OutcomeSubmittedAt");

        migrationBuilder.RenameColumn(
            name: "ProjectResultStatus",
            table: "PriceOffer",
            newName: "SalesOutcomeStatus");

        migrationBuilder.RenameColumn(
            name: "ProjectResultNote",
            table: "PriceOffer",
            newName: "SalesOutcomeNote");
    }
}
