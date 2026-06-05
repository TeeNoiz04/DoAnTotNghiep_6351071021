using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_SaleOrder_20250710_142700 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {


        migrationBuilder.CreateIndex(
            name: "IX_SaleOrderDetail_DPODetailId",
            table: "SaleOrderDetail",
            column: "DPODetailId");

        migrationBuilder.CreateIndex(
            name: "IX_SaleOrderDetail_StockCategoryId",
            table: "SaleOrderDetail",
            column: "StockCategoryId");

        migrationBuilder.AddForeignKey(
            name: "FK_SaleOrderDetail_DPODetail_DPODetailId",
            table: "SaleOrderDetail",
            column: "DPODetailId",
            principalTable: "DPODetail",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_SaleOrderDetail_StockCategory_StockCategoryId",
            table: "SaleOrderDetail",
            column: "StockCategoryId",
            principalTable: "StockCategory",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_SaleOrderDetail_DPODetail_DPODetailId",
            table: "SaleOrderDetail");

        migrationBuilder.DropForeignKey(
            name: "FK_SaleOrderDetail_StockCategory_StockCategoryId",
            table: "SaleOrderDetail");

        migrationBuilder.DropIndex(
            name: "IX_SaleOrderDetail_DPODetailId",
            table: "SaleOrderDetail");

        migrationBuilder.DropIndex(
            name: "IX_SaleOrderDetail_StockCategoryId",
            table: "SaleOrderDetail");


    }
}
