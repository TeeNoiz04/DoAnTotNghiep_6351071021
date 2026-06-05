using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_DPO_20250905_150800 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "GICNo",
            table: "DPO");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "GICNo",
            table: "DPO",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true);
    }
}
