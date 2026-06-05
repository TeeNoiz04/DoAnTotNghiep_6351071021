using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_PurchaseOrderDetail_20250814_150000 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Cargo_MEVNAddedRequest",
            table: "PurchaseOrderDetail",
            type: "nvarchar(500)",
            maxLength: 500,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Cargo_MachineNumber",
            table: "PurchaseOrderDetail",
            type: "nvarchar(200)",
            maxLength: 200,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Cargo_STCReply",
            table: "PurchaseOrderDetail",
            type: "nvarchar(max)",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Cargo_MEVNAddedRequest",
            table: "PurchaseOrderDetail");

        migrationBuilder.DropColumn(
            name: "Cargo_MachineNumber",
            table: "PurchaseOrderDetail");

        migrationBuilder.DropColumn(
            name: "Cargo_STCReply",
            table: "PurchaseOrderDetail");
    }
}
