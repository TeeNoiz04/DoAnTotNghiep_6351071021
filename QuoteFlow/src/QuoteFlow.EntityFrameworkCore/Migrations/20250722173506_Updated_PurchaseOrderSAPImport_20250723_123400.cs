using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_PurchaseOrderSAPImport_20250723_123400 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropPrimaryKey(
            name: "PK_PurchaseOrdersSapImport",
            table: "PurchaseOrdersSapImport");

        migrationBuilder.RenameTable(
            name: "PurchaseOrdersSapImport",
            newName: "PurchaseOrdersSAPImport");

        migrationBuilder.AlterColumn<Guid>(
            name: "ImportKey",
            table: "PurchaseOrdersSAPImport",
            type: "uniqueidentifier",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(50)",
            oldMaxLength: 50,
            oldNullable: true);

        migrationBuilder.AddColumn<string>(
            name: "FileName",
            table: "PurchaseOrdersSAPImport",
            type: "nvarchar(400)",
            maxLength: 400,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Note",
            table: "PurchaseOrdersSAPImport",
            type: "nvarchar(4000)",
            maxLength: 4000,
            nullable: true);

        migrationBuilder.AddPrimaryKey(
            name: "PK_PurchaseOrdersSAPImport",
            table: "PurchaseOrdersSAPImport",
            column: "Id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropPrimaryKey(
            name: "PK_PurchaseOrdersSAPImport",
            table: "PurchaseOrdersSAPImport");

        migrationBuilder.DropColumn(
            name: "FileName",
            table: "PurchaseOrdersSAPImport");

        migrationBuilder.DropColumn(
            name: "Note",
            table: "PurchaseOrdersSAPImport");

        migrationBuilder.RenameTable(
            name: "PurchaseOrdersSAPImport",
            newName: "PurchaseOrdersSapImport");

        migrationBuilder.AlterColumn<string>(
            name: "ImportKey",
            table: "PurchaseOrdersSapImport",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldNullable: true);

        migrationBuilder.AddPrimaryKey(
            name: "PK_PurchaseOrdersSapImport",
            table: "PurchaseOrdersSapImport",
            column: "Id");
    }
}
