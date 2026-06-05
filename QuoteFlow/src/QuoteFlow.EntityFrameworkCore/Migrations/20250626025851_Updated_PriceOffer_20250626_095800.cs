using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_PriceOffer_20250626_095800 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateIndex(
            name: "IX_PriceOfferDetail_GolfaCode",
            table: "PriceOfferDetail",
            column: "GolfaCode");

        migrationBuilder.CreateIndex(
            name: "IX_PriceOffer_Code",
            table: "PriceOffer",
            column: "PriceOffer_Code",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropIndex(
            name: "IX_PriceOfferDetail_GolfaCode",
            table: "PriceOfferDetail");

        migrationBuilder.DropIndex(
            name: "IX_PriceOffer_Code",
            table: "PriceOffer");
    }
}
