using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_PO_20250804_100300 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateIndex(
            name: "IX_PurchaseOrder_SupplierId",
            table: "PurchaseOrder",
            column: "SupplierId");

        migrationBuilder.AddForeignKey(
            name: "FK_PurchaseOrder_Supplier_SupplierId",
            table: "PurchaseOrder",
            column: "SupplierId",
            principalTable: "Supplier",
            principalColumn: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_PurchaseOrder_Supplier_SupplierId",
            table: "PurchaseOrder");

        migrationBuilder.DropIndex(
            name: "IX_PurchaseOrder_SupplierId",
            table: "PurchaseOrder");
    }
}
