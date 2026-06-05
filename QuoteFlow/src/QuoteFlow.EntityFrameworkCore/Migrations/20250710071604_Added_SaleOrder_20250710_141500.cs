using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Added_SaleOrder_20250710_141500 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "SaleOrder",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                SONo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                SOSAPNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                MaterialType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                BuyerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                BuyerCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                BuyerName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                StatusCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                StockCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                SO_VAT = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                Note = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
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
                table.PrimaryKey("PK_SaleOrder", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "SaleOrderDetail",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                SaleOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                DPODetailId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                StatusCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                GolfaCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                Qty = table.Column<int>(type: "int", nullable: true),
                Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                VAT = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                StockCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                Note = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                LockStockId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                table.PrimaryKey("PK_SaleOrderDetail", x => x.Id);
                table.ForeignKey(
                    name: "FK_SaleOrderDetail_SaleOrder_SaleOrderId",
                    column: x => x.SaleOrderId,
                    principalTable: "SaleOrder",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_SaleOrderDetail_SaleOrderId",
            table: "SaleOrderDetail",
            column: "SaleOrderId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "SaleOrderDetail");

        migrationBuilder.DropTable(
            name: "SaleOrder");
    }
}
