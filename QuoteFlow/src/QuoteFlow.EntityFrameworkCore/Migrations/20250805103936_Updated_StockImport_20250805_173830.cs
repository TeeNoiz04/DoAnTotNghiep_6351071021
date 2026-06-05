using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_StockImport_20250805_173830 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<DateTime>(
            name: "ATA",
            table: "StockImport",
            type: "datetime2",
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "ATD",
            table: "StockImport",
            type: "datetime2",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "BillNo",
            table: "StockImport",
            type: "nvarchar(200)",
            maxLength: 200,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "CDNo",
            table: "StockImport",
            type: "nvarchar(100)",
            maxLength: 100,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "DeliveryTerm",
            table: "StockImport",
            type: "nvarchar(200)",
            maxLength: 200,
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "ETA",
            table: "StockImport",
            type: "datetime2",
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "ETD",
            table: "StockImport",
            type: "datetime2",
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "InvoiceDate",
            table: "StockImport",
            type: "datetime2",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "InvoiceType",
            table: "StockImport",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "ReceivingReportDate",
            table: "StockImport",
            type: "datetime2",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "ShipmentMethod",
            table: "StockImport",
            type: "nvarchar(500)",
            maxLength: 500,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "SupplierCode",
            table: "StockImport",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "SupplierId",
            table: "StockImport",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "WHArrivalDate",
            table: "StockImport",
            type: "datetime2",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "ATA",
            table: "StockImport");

        migrationBuilder.DropColumn(
            name: "ATD",
            table: "StockImport");

        migrationBuilder.DropColumn(
            name: "BillNo",
            table: "StockImport");

        migrationBuilder.DropColumn(
            name: "CDNo",
            table: "StockImport");

        migrationBuilder.DropColumn(
            name: "DeliveryTerm",
            table: "StockImport");

        migrationBuilder.DropColumn(
            name: "ETA",
            table: "StockImport");

        migrationBuilder.DropColumn(
            name: "ETD",
            table: "StockImport");

        migrationBuilder.DropColumn(
            name: "InvoiceDate",
            table: "StockImport");

        migrationBuilder.DropColumn(
            name: "InvoiceType",
            table: "StockImport");

        migrationBuilder.DropColumn(
            name: "ReceivingReportDate",
            table: "StockImport");

        migrationBuilder.DropColumn(
            name: "ShipmentMethod",
            table: "StockImport");

        migrationBuilder.DropColumn(
            name: "SupplierCode",
            table: "StockImport");

        migrationBuilder.DropColumn(
            name: "SupplierId",
            table: "StockImport");

        migrationBuilder.DropColumn(
            name: "WHArrivalDate",
            table: "StockImport");
    }
}
