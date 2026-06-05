using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_SO_20250721_153700 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<decimal>(
            name: "AmountIncludeExtrafee",
            table: "SaleOrderDetail",
            type: "decimal(18,2)",
            nullable: true);

        migrationBuilder.AddColumn<decimal>(
            name: "Extrafee",
            table: "SaleOrderDetail",
            type: "decimal(18,2)",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "AmountIncludeExtrafee",
            table: "SaleOrderDetail");

        migrationBuilder.DropColumn(
            name: "Extrafee",
            table: "SaleOrderDetail");
    }
}
