using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Added_PurchaseOrder_20250710_151700 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "PurchaseOrder",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                PONo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                PODate = table.Column<DateTime>(type: "datetime2", nullable: true),
                POSAPNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                POSAPDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                StatusCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                CreateSource = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                MaterialType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                SupplierBUId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                SupplierBUCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                Currency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                EPA = table.Column<bool>(type: "bit", nullable: false),
                OurRef = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatorUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                CreatorName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModifierUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                LastModifierName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PurchaseOrder", x => x.Id);
                table.ForeignKey(
                    name: "FK_PurchaseOrder_Supplier_BU_SupplierBUId",
                    column: x => x.SupplierBUId,
                    principalTable: "Supplier_BU",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateTable(
            name: "PurchaseOrderDetail",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                PurchaseOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                GolfaCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                StatusCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                Qty = table.Column<int>(type: "int", nullable: true),
                Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                AmountVND = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                Note = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                ProjectCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                AccountNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                QtyImported = table.Column<int>(type: "int", nullable: true),
                QtyLocked = table.Column<int>(type: "int", nullable: true),
                QtyAvailable = table.Column<int>(type: "int", nullable: true),
                QtyNeedImport = table.Column<int>(type: "int", nullable: true),
                LeadTime = table.Column<int>(type: "int", nullable: true),
                Maxlot = table.Column<int>(type: "int", nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatorUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                CreatorName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModifierUsername = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                LastModifierName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PurchaseOrderDetail", x => x.Id);
                table.ForeignKey(
                    name: "FK_PurchaseOrderDetail_PurchaseOrder_PurchaseOrderId",
                    column: x => x.PurchaseOrderId,
                    principalTable: "PurchaseOrder",
                    principalColumn: "Id");
            });

        migrationBuilder.CreateIndex(
            name: "IX_PurchaseOrder_SupplierBUId",
            table: "PurchaseOrder",
            column: "SupplierBUId");

        migrationBuilder.CreateIndex(
            name: "IX_PurchaseOrderDetail_PurchaseOrderId",
            table: "PurchaseOrderDetail",
            column: "PurchaseOrderId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "PurchaseOrderDetail");

        migrationBuilder.DropTable(
            name: "PurchaseOrder");
    }
}
