using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Added_StockImportDetails_20250710_135530 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "StockImportDetails",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                StockImportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                InvoiceNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                ItemModel = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                MaterialCode = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                Unit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                Qty = table.Column<int>(type: "int", nullable: true),
                Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                GensanchiNM = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                ETA = table.Column<DateTime>(type: "datetime2", nullable: true),
                ETD = table.Column<DateTime>(type: "datetime2", nullable: true),
                ShipmentMethod = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                BillNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                MachineNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                PONo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                CDNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                Note = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                CreatorUsername = table.Column<string>(type: "nvarchar(max)", nullable: true),
                CreatorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                CreationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                LastModifierUsername = table.Column<string>(type: "nvarchar(max)", nullable: true),
                LastModifierName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_StockImportDetails", x => x.Id);
                table.ForeignKey(
                    name: "FK_StockImportDetails_StockImport_StockImportId",
                    column: x => x.StockImportId,
                    principalTable: "StockImport",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_StockImportDetails_StockImportId",
            table: "StockImportDetails",
            column: "StockImportId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "StockImportDetails");
    }
}
