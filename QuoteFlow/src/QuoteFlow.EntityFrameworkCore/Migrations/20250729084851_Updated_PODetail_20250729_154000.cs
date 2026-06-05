using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_PODetail_20250729_154000 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "QtyNeedImport",
            table: "PurchaseOrderDetail");



        migrationBuilder.AddColumn<string>(
            name: "Customer",
            table: "PurchaseOrderDetail",
            type: "nvarchar(255)",
            maxLength: 255,
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "RequestETA",
            table: "PurchaseOrderDetail",
            type: "datetime2",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "Urgent",
            table: "PurchaseOrderDetail",
            type: "bit",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {

        migrationBuilder.DropColumn(
            name: "Customer",
            table: "PurchaseOrderDetail");

        migrationBuilder.DropColumn(
            name: "RequestETA",
            table: "PurchaseOrderDetail");

        migrationBuilder.DropColumn(
            name: "Urgent",
            table: "PurchaseOrderDetail");



        migrationBuilder.AddColumn<int>(
            name: "QtyNeedImport",
            table: "PurchaseOrderDetail",
            type: "int",
            nullable: true);
    }
}
