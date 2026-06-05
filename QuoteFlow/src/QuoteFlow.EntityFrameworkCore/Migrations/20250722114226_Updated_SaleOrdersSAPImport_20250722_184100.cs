using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_SaleOrdersSAPImport_20250722_184100 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropPrimaryKey(
            name: "PK_SaleOrdersSapImport",
            table: "SaleOrdersSapImport");

        migrationBuilder.DropColumn(
            name: "DODate",
            table: "SaleOrdersSapImport");

        migrationBuilder.DropColumn(
            name: "DONo",
            table: "SaleOrdersSapImport");

        migrationBuilder.RenameTable(
            name: "SaleOrdersSapImport",
            newName: "SaleOrdersSAPImport");

        migrationBuilder.RenameColumn(
            name: "Deleted",
            table: "SaleOrdersSAPImport",
            newName: "IsDeleted");

        migrationBuilder.RenameColumn(
            name: "DONote",
            table: "SaleOrdersSAPImport",
            newName: "Note");

        migrationBuilder.AddColumn<string>(
            name: "FileName",
            table: "SaleOrdersSAPImport",
            type: "nvarchar(400)",
            maxLength: 400,
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "ImportKey",
            table: "SaleOrdersSAPImport",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddPrimaryKey(
            name: "PK_SaleOrdersSAPImport",
            table: "SaleOrdersSAPImport",
            column: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropPrimaryKey(
            name: "PK_SaleOrdersSAPImport",
            table: "SaleOrdersSAPImport");

        migrationBuilder.DropColumn(
            name: "FileName",
            table: "SaleOrdersSAPImport");

        migrationBuilder.DropColumn(
            name: "ImportKey",
            table: "SaleOrdersSAPImport");

        migrationBuilder.RenameTable(
            name: "SaleOrdersSAPImport",
            newName: "SaleOrdersSapImport");

        migrationBuilder.RenameColumn(
            name: "Note",
            table: "SaleOrdersSapImport",
            newName: "DONote");

        migrationBuilder.RenameColumn(
            name: "IsDeleted",
            table: "SaleOrdersSapImport",
            newName: "Deleted");

        migrationBuilder.AddColumn<DateTime>(
            name: "DODate",
            table: "SaleOrdersSapImport",
            type: "datetime2",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "DONo",
            table: "SaleOrdersSapImport",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true);

        migrationBuilder.AddPrimaryKey(
            name: "PK_SaleOrdersSapImport",
            table: "SaleOrdersSapImport",
            column: "Id");
    }
}
