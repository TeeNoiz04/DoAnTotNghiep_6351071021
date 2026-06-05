using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_PurchaseOrderLockShipment_161800 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_PurchaseOrder_Supplier_BU_SupplierBUId",
            table: "PurchaseOrder");

        migrationBuilder.AddColumn<string>(
            name: "DPONo",
            table: "PurchaseOrderLockShipment",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true);

        migrationBuilder.AddForeignKey(
            name: "FK_PurchaseOrder_Supplier_BU_SupplierBUId",
            table: "PurchaseOrder",
            column: "SupplierBUId",
            principalTable: "Supplier_BU",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_PurchaseOrder_Supplier_BU_SupplierBUId",
            table: "PurchaseOrder");

        migrationBuilder.DropColumn(
            name: "DPONo",
            table: "PurchaseOrderLockShipment");

        migrationBuilder.AddForeignKey(
            name: "FK_PurchaseOrder_Supplier_BU_SupplierBUId",
            table: "PurchaseOrder",
            column: "SupplierBUId",
            principalTable: "Supplier_BU",
            principalColumn: "Id");
    }
}
