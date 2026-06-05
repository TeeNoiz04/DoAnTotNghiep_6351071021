using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_DistributorTarget_20250808_111830 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropPrimaryKey(
            name: "PK_DistributorTarget",
            table: "DistributorTarget");

        migrationBuilder.RenameTable(
            name: "DistributorTarget",
            newName: "BuyerTarget");

        migrationBuilder.AddColumn<string>(
            name: "BuyerName",
            table: "BuyerTarget",
            type: "nvarchar(400)",
            maxLength: 400,
            nullable: true);

        migrationBuilder.AddPrimaryKey(
            name: "PK_BuyerTarget",
            table: "BuyerTarget",
            column: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropPrimaryKey(
            name: "PK_BuyerTarget",
            table: "BuyerTarget");

        migrationBuilder.DropColumn(
            name: "BuyerName",
            table: "BuyerTarget");

        migrationBuilder.RenameTable(
            name: "BuyerTarget",
            newName: "DistributorTarget");

        migrationBuilder.AddPrimaryKey(
            name: "PK_DistributorTarget",
            table: "DistributorTarget",
            column: "Id");
    }
}
