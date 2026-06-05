using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_PurchaseOrder_20250715_135000 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {


        migrationBuilder.AddColumn<string>(
            name: "SupplierCode",
            table: "PurchaseOrder",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "SupplierId",
            table: "PurchaseOrder",
            type: "uniqueidentifier",
            nullable: true);


    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {


        migrationBuilder.DropColumn(
            name: "SupplierCode",
            table: "PurchaseOrder");

        migrationBuilder.DropColumn(
            name: "SupplierId",
            table: "PurchaseOrder");


    }
}
