using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_PriceOffer_20250702 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateIndex(
            name: "IX_Discussion_PriceOfferId",
            table: "Discussion",
            column: "PriceOfferId");

        migrationBuilder.AddForeignKey(
            name: "FK_Discussion_PriceOffer_PriceOfferId",
            table: "Discussion",
            column: "PriceOfferId",
            principalTable: "PriceOffer",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Discussion_PriceOffer_PriceOfferId",
            table: "Discussion");

        migrationBuilder.DropIndex(
            name: "IX_Discussion_PriceOfferId",
            table: "Discussion");
    }
}
