using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_StockImport_Allocation_20250721_201000 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {


        migrationBuilder.CreateTable(
            name: "StockImport_Allocation",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                StockImportId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                StockImportDetail_Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                InvoiceNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                PODetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                PONo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                DPODetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                DPONo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                MaterialCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                Qty_Import = table.Column<int>(type: "int", nullable: true),
                Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                Qty_Requested = table.Column<int>(type: "int", nullable: true),
                Qty_Import_ForAllocation = table.Column<int>(type: "int", nullable: true),
                Qty_Allocation = table.Column<int>(type: "int", nullable: true),
                Allocation_Order = table.Column<int>(type: "int", nullable: true),
                AllocationStep = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                Note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
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
                table.PrimaryKey("PK_StockImport_Allocation", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "StockImport_Allocation");

    }
}
