using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_DPODetail_20250721_152500 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "DPODetail.Extrafee_Used_InSO",
            table: "DPODetail",
            newName: "Extrafee_Used_InSO");

        migrationBuilder.RenameColumn(
            name: "DPODetail.Extrafee_Note",
            table: "DPODetail",
            newName: "Extrafee_Note");

        migrationBuilder.RenameColumn(
            name: "DPODetail.Extrafee_Available",
            table: "DPODetail",
            newName: "Extrafee_Available");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.RenameColumn(
            name: "Extrafee_Used_InSO",
            table: "DPODetail",
            newName: "DPODetail.Extrafee_Used_InSO");

        migrationBuilder.RenameColumn(
            name: "Extrafee_Note",
            table: "DPODetail",
            newName: "DPODetail.Extrafee_Note");

        migrationBuilder.RenameColumn(
            name: "Extrafee_Available",
            table: "DPODetail",
            newName: "DPODetail.Extrafee_Available");
    }
}
