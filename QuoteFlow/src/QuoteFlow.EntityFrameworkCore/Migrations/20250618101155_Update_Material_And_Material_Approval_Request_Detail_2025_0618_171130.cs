using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Update_Material_And_Material_Approval_Request_Detail_2025_0618_171130 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "SupplierCode",
            table: "Materials",
            type: "nvarchar(400)",
            maxLength: 400,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "SupplierCode",
            table: "MaterialApprovalRequestDetail",
            type: "nvarchar(400)",
            maxLength: 400,
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "SupplierCode",
            table: "Materials");

        migrationBuilder.DropColumn(
            name: "SupplierCode",
            table: "MaterialApprovalRequestDetail");
    }
}
