using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_PriceOffer_20250618_103400 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "EUIndustryDescription",
            table: "PriceOffer",
            type: "nvarchar(500)",
            maxLength: 500,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "KeyAccountClassDescription",
            table: "PriceOffer",
            type: "nvarchar(500)",
            maxLength: 500,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "KeyAccountTypeDescription",
            table: "PriceOffer",
            type: "nvarchar(500)",
            maxLength: 500,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "LocationDescription",
            table: "PriceOffer",
            type: "nvarchar(500)",
            maxLength: 500,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "ProjectTypeDescription",
            table: "PriceOffer",
            type: "nvarchar(500)",
            maxLength: 500,
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "EUIndustryDescription",
            table: "PriceOffer");

        migrationBuilder.DropColumn(
            name: "KeyAccountClassDescription",
            table: "PriceOffer");

        migrationBuilder.DropColumn(
            name: "KeyAccountTypeDescription",
            table: "PriceOffer");

        migrationBuilder.DropColumn(
            name: "LocationDescription",
            table: "PriceOffer");

        migrationBuilder.DropColumn(
            name: "ProjectTypeDescription",
            table: "PriceOffer");
    }
}
