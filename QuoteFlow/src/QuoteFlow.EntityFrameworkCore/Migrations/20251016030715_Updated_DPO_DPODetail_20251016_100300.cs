using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations
{
    /// <inheritdoc />
    public partial class Updated_DPO_DPODetail_20251016_100300 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MaterialCode",
                table: "SaleOrdersSAPImport",
                newName: "GolfaCode");

            migrationBuilder.AddColumn<string>(
                name: "GICNo",
                table: "SaleOrdersSAPImport",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GolfaCode",
                table: "HistoryTracking",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<decimal>(
                name: "LandedCost",
                table: "DPODetail",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReferenceDocDate",
                table: "DPO",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GICNo",
                table: "SaleOrdersSAPImport");

            migrationBuilder.DropColumn(
                name: "LandedCost",
                table: "DPODetail");

            migrationBuilder.DropColumn(
                name: "ReferenceDocDate",
                table: "DPO");

            migrationBuilder.RenameColumn(
                name: "GolfaCode",
                table: "SaleOrdersSAPImport",
                newName: "MaterialCode");

            migrationBuilder.AlterColumn<string>(
                name: "GolfaCode",
                table: "HistoryTracking",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
