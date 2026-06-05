using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_DPODetail_20250721_145400 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "AccountNo",
            table: "DPODetail",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true);

        migrationBuilder.AddColumn<decimal>(
            name: "DPODetail.Extrafee_Available",
            table: "DPODetail",
            type: "decimal(18,2)",
            precision: 18,
            scale: 2,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "DPODetail.Extrafee_Note",
            table: "DPODetail",
            type: "nvarchar(4000)",
            maxLength: 4000,
            nullable: true);

        migrationBuilder.AddColumn<decimal>(
            name: "DPODetail.Extrafee_Used_InSO",
            table: "DPODetail",
            type: "decimal(18,2)",
            precision: 18,
            scale: 2,
            nullable: true);

        migrationBuilder.AddColumn<decimal>(
            name: "Extrafee",
            table: "DPODetail",
            type: "decimal(18,2)",
            precision: 18,
            scale: 2,
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "AccountNo",
            table: "DPODetail");

        migrationBuilder.DropColumn(
            name: "DPODetail.Extrafee_Available",
            table: "DPODetail");

        migrationBuilder.DropColumn(
            name: "DPODetail.Extrafee_Note",
            table: "DPODetail");

        migrationBuilder.DropColumn(
            name: "DPODetail.Extrafee_Used_InSO",
            table: "DPODetail");

        migrationBuilder.DropColumn(
            name: "Extrafee",
            table: "DPODetail");
    }
}
