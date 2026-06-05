using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_PriceOfferDetail_20250619_153200 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<decimal>(
            name: "MaxMangerOfferPrice",
            table: "PriceOfferDetail",
            type: "decimal(18,2)",
            precision: 18,
            scale: 2,
            nullable: true);

        migrationBuilder.AddColumn<decimal>(
            name: "MaxSalesOfferPrice",
            table: "PriceOfferDetail",
            type: "decimal(18,2)",
            precision: 18,
            scale: 2,
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "MaxMangerOfferPrice",
            table: "PriceOfferDetail");

        migrationBuilder.DropColumn(
            name: "MaxSalesOfferPrice",
            table: "PriceOfferDetail");
    }
}
