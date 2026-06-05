using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_PriceOfferDetail_20250619_154400 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "PriceOfferMargin",
            table: "PriceOfferDetail",
            newName: "PriceOfferDetailMargin");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "PriceOfferDetailMargin",
            table: "PriceOfferDetail",
            newName: "PriceOfferMargin");
    }
}
