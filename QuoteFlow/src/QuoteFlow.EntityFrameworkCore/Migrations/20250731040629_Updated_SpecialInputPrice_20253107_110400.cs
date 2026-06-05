using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_SpecialInputPrice_20253107_110400 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateIndex(
            name: "IX_SpecialInputPriceDetail_SpecialInputPriceId",
            table: "SpecialInputPriceDetail",
            column: "SpecialInputPriceId");

        migrationBuilder.AddForeignKey(
            name: "FK_SpecialInputPriceDetail_SpecialInputPrice_SpecialInputPriceId",
            table: "SpecialInputPriceDetail",
            column: "SpecialInputPriceId",
            principalTable: "SpecialInputPrice",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_SpecialInputPriceDetail_SpecialInputPrice_SpecialInputPriceId",
            table: "SpecialInputPriceDetail");

        migrationBuilder.DropIndex(
            name: "IX_SpecialInputPriceDetail_SpecialInputPriceId",
            table: "SpecialInputPriceDetail");
    }
}
