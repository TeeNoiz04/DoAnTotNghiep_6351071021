using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_SpecialInputPrice_20250814_135300 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Currency",
            table: "SpecialInputPrice",
            type: "nvarchar(10)",
            maxLength: 10,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "MaterialType",
            table: "SpecialInputPrice",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "SupplierBUId",
            table: "SpecialInputPrice",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "SupplierId",
            table: "SpecialInputPrice",
            type: "uniqueidentifier",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Currency",
            table: "SpecialInputPrice");

        migrationBuilder.DropColumn(
            name: "MaterialType",
            table: "SpecialInputPrice");

        migrationBuilder.DropColumn(
            name: "SupplierBUId",
            table: "SpecialInputPrice");

        migrationBuilder.DropColumn(
            name: "SupplierId",
            table: "SpecialInputPrice");
    }
}
