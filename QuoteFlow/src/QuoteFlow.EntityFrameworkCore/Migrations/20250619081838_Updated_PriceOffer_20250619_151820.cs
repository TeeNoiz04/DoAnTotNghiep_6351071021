using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_PriceOffer_20250619_151820 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "TotalMEVNOfferPrice",
            table: "PriceOffer");

        migrationBuilder.RenameColumn(
            name: "TotalAmount",
            table: "PriceOffer",
            newName: "TotalMEVNOfferAmount");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "TotalMEVNOfferAmount",
            table: "PriceOffer",
            newName: "TotalAmount");

        migrationBuilder.AddColumn<decimal>(
            name: "TotalMEVNOfferPrice",
            table: "PriceOffer",
            type: "decimal(18,2)",
            precision: 18,
            scale: 2,
            nullable: false,
            defaultValue: 0m);
    }
}
