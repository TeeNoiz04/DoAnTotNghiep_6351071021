using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_DPO_20250626_134800 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "DPOSubType",
            table: "DPO");

        migrationBuilder.AddColumn<string>(
            name: "GICNo",
            table: "DPO",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "GICProcess",
            table: "DPO",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "GICType",
            table: "DPO",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "ReferenceDoc",
            table: "DPO",
            type: "nvarchar(max)",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "GICNo",
            table: "DPO");

        migrationBuilder.DropColumn(
            name: "GICProcess",
            table: "DPO");

        migrationBuilder.DropColumn(
            name: "GICType",
            table: "DPO");

        migrationBuilder.DropColumn(
            name: "ReferenceDoc",
            table: "DPO");

        migrationBuilder.AddColumn<string>(
            name: "DPOSubType",
            table: "DPO",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true);
    }
}
