using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_DPODetail_20250612_184800 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "StatusCode",
            table: "DPODetail");

        migrationBuilder.AddColumn<string>(
            name: "Status",
            table: "DPODetail",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Status",
            table: "DPODetail");

        migrationBuilder.AddColumn<string>(
            name: "StatusCode",
            table: "DPODetail",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: false,
            defaultValue: "");
    }
}
