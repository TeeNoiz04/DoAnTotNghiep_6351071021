using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_Material_MaterialDetail_20250725_151530 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "CargoNote",
            table: "Materials",
            type: "nvarchar(500)",
            maxLength: 500,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "QRCode",
            table: "Materials",
            type: "nvarchar(500)",
            maxLength: 500,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Size",
            table: "Materials",
            type: "nvarchar(500)",
            maxLength: 500,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Weight",
            table: "Materials",
            type: "nvarchar(500)",
            maxLength: 500,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "CargoNote",
            table: "MaterialApprovalRequestDetail",
            type: "nvarchar(500)",
            maxLength: 500,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "QRCode",
            table: "MaterialApprovalRequestDetail",
            type: "nvarchar(500)",
            maxLength: 500,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Size",
            table: "MaterialApprovalRequestDetail",
            type: "nvarchar(500)",
            maxLength: 500,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Weight",
            table: "MaterialApprovalRequestDetail",
            type: "nvarchar(500)",
            maxLength: 500,
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "CargoNote",
            table: "Materials");

        migrationBuilder.DropColumn(
            name: "QRCode",
            table: "Materials");

        migrationBuilder.DropColumn(
            name: "Size",
            table: "Materials");

        migrationBuilder.DropColumn(
            name: "Weight",
            table: "Materials");

        migrationBuilder.DropColumn(
            name: "CargoNote",
            table: "MaterialApprovalRequestDetail");

        migrationBuilder.DropColumn(
            name: "QRCode",
            table: "MaterialApprovalRequestDetail");

        migrationBuilder.DropColumn(
            name: "Size",
            table: "MaterialApprovalRequestDetail");

        migrationBuilder.DropColumn(
            name: "Weight",
            table: "MaterialApprovalRequestDetail");
    }
}
