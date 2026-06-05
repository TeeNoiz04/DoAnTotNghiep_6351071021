using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_SupplierBU_20250709_103500 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {


        migrationBuilder.CreateIndex(
            name: "IX_Supplier_BU_SupplierId",
            table: "Supplier_BU",
            column: "SupplierId");

        migrationBuilder.AddForeignKey(
            name: "FK_Supplier_BU_Supplier_SupplierId",
            table: "Supplier_BU",
            column: "SupplierId",
            principalTable: "Supplier",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {

        migrationBuilder.DropForeignKey(
            name: "FK_Supplier_BU_Supplier_SupplierId",
            table: "Supplier_BU");


        migrationBuilder.DropIndex(
            name: "IX_Supplier_BU_SupplierId",
            table: "Supplier_BU");

    }
}
