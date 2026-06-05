using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_SO_20250725_170000 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "BillingNo",
            table: "SaleOrder",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "DOSAPNo",
            table: "SaleOrder",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "DeliveryDate",
            table: "SaleOrder",
            type: "datetime2",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "BillingNo",
            table: "SaleOrder");

        migrationBuilder.DropColumn(
            name: "DOSAPNo",
            table: "SaleOrder");

        migrationBuilder.DropColumn(
            name: "DeliveryDate",
            table: "SaleOrder");
    }
}
