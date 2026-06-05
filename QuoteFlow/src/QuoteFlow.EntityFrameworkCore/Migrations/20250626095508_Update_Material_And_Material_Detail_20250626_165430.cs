using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Update_Material_And_Material_Detail_20250626_165430 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "MaterialClass",
            table: "Materials",
            type: "nvarchar(400)",
            maxLength: 400,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "MaterialClass",
            table: "MaterialApprovalRequestDetail",
            type: "nvarchar(400)",
            maxLength: 400,
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "MaterialClass",
            table: "Materials");

        migrationBuilder.DropColumn(
            name: "MaterialClass",
            table: "MaterialApprovalRequestDetail");
    }
}
