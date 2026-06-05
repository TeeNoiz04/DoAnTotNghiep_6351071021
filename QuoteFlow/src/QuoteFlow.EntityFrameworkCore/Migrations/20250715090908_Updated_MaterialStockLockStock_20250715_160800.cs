using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_MaterialStockLockStock_20250715_160800 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateIndex(
            name: "IX_MaterialStock_LockStock_StockCategoryId",
            table: "MaterialStock_LockStock",
            column: "StockCategoryId");

        migrationBuilder.AddForeignKey(
            name: "FK_MaterialStock_LockStock_StockCategory_StockCategoryId",
            table: "MaterialStock_LockStock",
            column: "StockCategoryId",
            principalTable: "StockCategory",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_MaterialStock_LockStock_StockCategory_StockCategoryId",
            table: "MaterialStock_LockStock");

        migrationBuilder.DropIndex(
            name: "IX_MaterialStock_LockStock_StockCategoryId",
            table: "MaterialStock_LockStock");
    }
}
