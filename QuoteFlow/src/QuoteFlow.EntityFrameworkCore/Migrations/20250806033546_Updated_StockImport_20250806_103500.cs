using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_StockImport_20250806_103500 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<decimal>(
            name: "TotalAmount",
            table: "StockImport",
            type: "decimal(18,2)",
            nullable: true);

        migrationBuilder.AddColumn<int>(
            name: "TotalQty",
            table: "StockImport",
            type: "int",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "TotalAmount",
            table: "StockImport");

        migrationBuilder.DropColumn(
            name: "TotalQty",
            table: "StockImport");
    }
}
