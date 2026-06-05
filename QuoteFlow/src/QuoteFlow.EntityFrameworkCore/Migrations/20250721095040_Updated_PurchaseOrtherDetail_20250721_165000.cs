using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_PurchaseOrtherDetail_20250721_165000 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "PODetalCode",
            table: "PurchaseOrderDetail",
            newName: "PODetailCode");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "PODetailCode",
            table: "PurchaseOrderDetail",
            newName: "PODetalCode");
    }
}
