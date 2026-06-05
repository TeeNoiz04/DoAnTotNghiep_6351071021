using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_PriceOffer_20250619_152600 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "MarginIssue",
            table: "PriceOffer",
            type: "int",
            nullable: true);

        migrationBuilder.AddColumn<decimal>(
            name: "SPO_DiscountRatio",
            table: "PriceOffer",
            type: "decimal(18,2)",
            precision: 18,
            scale: 2,
            nullable: true);

        migrationBuilder.AddColumn<decimal>(
            name: "SPO_DiscountRatio_CFG",
            table: "PriceOffer",
            type: "decimal(18,2)",
            precision: 18,
            scale: 2,
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "MarginIssue",
            table: "PriceOffer");

        migrationBuilder.DropColumn(
            name: "SPO_DiscountRatio",
            table: "PriceOffer");

        migrationBuilder.DropColumn(
            name: "SPO_DiscountRatio_CFG",
            table: "PriceOffer");
    }
}
