using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_PriceOffer_20250619_134800 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<decimal>(
            name: "TotalMEVNOfferPrice",
            table: "PriceOffer",
            type: "decimal(18,2)",
            precision: 18,
            scale: 2,
            nullable: false,
            defaultValue: 0m);

        migrationBuilder.AddColumn<decimal>(
            name: "TotalPriceToCustomer",
            table: "PriceOffer",
            type: "decimal(18,2)",
            precision: 18,
            scale: 2,
            nullable: false,
            defaultValue: 0m);

        migrationBuilder.AddColumn<decimal>(
            name: "TotalRequestedAmount",
            table: "PriceOffer",
            type: "decimal(18,2)",
            precision: 18,
            scale: 2,
            nullable: false,
            defaultValue: 0m);

        migrationBuilder.AddColumn<decimal>(
            name: "TotalStandardAmount",
            table: "PriceOffer",
            type: "decimal(18,2)",
            precision: 18,
            scale: 2,
            nullable: false,
            defaultValue: 0m);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "TotalMEVNOfferPrice",
            table: "PriceOffer");

        migrationBuilder.DropColumn(
            name: "TotalPriceToCustomer",
            table: "PriceOffer");

        migrationBuilder.DropColumn(
            name: "TotalRequestedAmount",
            table: "PriceOffer");

        migrationBuilder.DropColumn(
            name: "TotalStandardAmount",
            table: "PriceOffer");
    }
}
