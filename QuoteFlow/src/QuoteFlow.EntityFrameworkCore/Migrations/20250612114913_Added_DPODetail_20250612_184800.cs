using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Added_DPODetail_20250612_184800 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "DPODetail",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                DPOId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                StatusCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                GolfaCode = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                Model = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                Spec1 = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                Spec2 = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                Qty = table.Column<int>(type: "int", nullable: true),
                UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                RequestedETA = table.Column<DateTime>(type: "datetime2", nullable: true),
                SPOId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                SPOCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                CustomerTaxCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                CustomerName = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                LockStock = table.Column<int>(type: "int", nullable: true),
                LockStockSO = table.Column<int>(type: "int", nullable: true),
                LockShipment = table.Column<int>(type: "int", nullable: true),
                Delivered = table.Column<int>(type: "int", nullable: true),
                NeedDelivery = table.Column<int>(type: "int", nullable: true),
                Note = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
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
                table.PrimaryKey("PK_DPODetail", x => x.Id);
                table.ForeignKey(
                    name: "FK_DPODetail_DPO_DPOId",
                    column: x => x.DPOId,
                    principalTable: "DPO",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_DPODetail_DPOId",
            table: "DPODetail",
            column: "DPOId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "DPODetail");
    }
}
