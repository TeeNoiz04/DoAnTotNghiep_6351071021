using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_PriceOffer_20250624_095200 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "SpecialInputPriceAppliedTime",
            table: "PriceOffer",
            newName: "SpecialInputPriceAssignedTime");

        migrationBuilder.AddColumn<string>(
            name: "SpecialInputPriceAccountName",
            table: "PriceOffer",
            type: "nvarchar(500)",
            maxLength: 500,
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "SpecialInputPriceAccountName",
            table: "PriceOffer");

        migrationBuilder.RenameColumn(
            name: "SpecialInputPriceAssignedTime",
            table: "PriceOffer",
            newName: "SpecialInputPriceAppliedTime");
    }
}
