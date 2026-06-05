using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_GIC_SaleOrder_SaleOrderSAPImport : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<decimal>(
            name: "GICAmountLandingCost",
            table: "SaleOrdersSAPImport",
            type: "decimal(18,2)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "GICAssetClass",
            table: "SaleOrdersSAPImport",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "GICAssetName",
            table: "SaleOrdersSAPImport",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "GICGivDate",
            table: "SaleOrdersSAPImport",
            type: "datetime2",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "GICGivNo",
            table: "SaleOrdersSAPImport",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true);

        migrationBuilder.AddColumn<decimal>(
            name: "GICLandingCost",
            table: "SaleOrdersSAPImport",
            type: "decimal(18,2)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "GICLocation",
            table: "SaleOrdersSAPImport",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "GICMainAssetCode",
            table: "SaleOrdersSAPImport",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "GICPORNo",
            table: "SaleOrdersSAPImport",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "GICPRNo",
            table: "SaleOrdersSAPImport",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "GICReservationNo",
            table: "SaleOrdersSAPImport",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "GICSalesPIC",
            table: "SaleOrdersSAPImport",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "GICSubAssetCode",
            table: "SaleOrdersSAPImport",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "MaterialCode",
            table: "SaleOrdersSAPImport",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "ModelName",
            table: "SaleOrdersSAPImport",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "SOType",
            table: "SaleOrdersSAPImport",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true);

        migrationBuilder.AddColumn<decimal>(
            name: "SAPGICAmountLandingCost",
            table: "SaleOrder",
            type: "decimal(18,2)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "SAPGICAssetClass",
            table: "SaleOrder",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "SAPGICAssetName",
            table: "SaleOrder",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "SAPGICGivDate",
            table: "SaleOrder",
            type: "datetime2",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "SAPGICGivNo",
            table: "SaleOrder",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true);

        migrationBuilder.AddColumn<decimal>(
            name: "SAPGICLandingCost",
            table: "SaleOrder",
            type: "decimal(18,2)",
            maxLength: 50,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "SAPGICLocation",
            table: "SaleOrder",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "SAPGICMainAssetCode",
            table: "SaleOrder",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "SAPGICPORNo",
            table: "SaleOrder",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "SAPGICPRNo",
            table: "SaleOrder",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "SAPGICReservationNo",
            table: "SaleOrder",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "SAPGICSalesPIC",
            table: "SaleOrder",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "SAPGICSubAssetCode",
            table: "SaleOrder",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "GICAmountLandingCost",
            table: "SaleOrdersSAPImport");

        migrationBuilder.DropColumn(
            name: "GICAssetClass",
            table: "SaleOrdersSAPImport");

        migrationBuilder.DropColumn(
            name: "GICAssetName",
            table: "SaleOrdersSAPImport");

        migrationBuilder.DropColumn(
            name: "GICGivDate",
            table: "SaleOrdersSAPImport");

        migrationBuilder.DropColumn(
            name: "GICGivNo",
            table: "SaleOrdersSAPImport");

        migrationBuilder.DropColumn(
            name: "GICLandingCost",
            table: "SaleOrdersSAPImport");

        migrationBuilder.DropColumn(
            name: "GICLocation",
            table: "SaleOrdersSAPImport");

        migrationBuilder.DropColumn(
            name: "GICMainAssetCode",
            table: "SaleOrdersSAPImport");

        migrationBuilder.DropColumn(
            name: "GICPORNo",
            table: "SaleOrdersSAPImport");

        migrationBuilder.DropColumn(
            name: "GICPRNo",
            table: "SaleOrdersSAPImport");

        migrationBuilder.DropColumn(
            name: "GICReservationNo",
            table: "SaleOrdersSAPImport");

        migrationBuilder.DropColumn(
            name: "GICSalesPIC",
            table: "SaleOrdersSAPImport");

        migrationBuilder.DropColumn(
            name: "GICSubAssetCode",
            table: "SaleOrdersSAPImport");

        migrationBuilder.DropColumn(
            name: "MaterialCode",
            table: "SaleOrdersSAPImport");

        migrationBuilder.DropColumn(
            name: "ModelName",
            table: "SaleOrdersSAPImport");

        migrationBuilder.DropColumn(
            name: "SOType",
            table: "SaleOrdersSAPImport");

        migrationBuilder.DropColumn(
            name: "SAPGICAmountLandingCost",
            table: "SaleOrder");

        migrationBuilder.DropColumn(
            name: "SAPGICAssetClass",
            table: "SaleOrder");

        migrationBuilder.DropColumn(
            name: "SAPGICAssetName",
            table: "SaleOrder");

        migrationBuilder.DropColumn(
            name: "SAPGICGivDate",
            table: "SaleOrder");

        migrationBuilder.DropColumn(
            name: "SAPGICGivNo",
            table: "SaleOrder");

        migrationBuilder.DropColumn(
            name: "SAPGICLandingCost",
            table: "SaleOrder");

        migrationBuilder.DropColumn(
            name: "SAPGICLocation",
            table: "SaleOrder");

        migrationBuilder.DropColumn(
            name: "SAPGICMainAssetCode",
            table: "SaleOrder");

        migrationBuilder.DropColumn(
            name: "SAPGICPORNo",
            table: "SaleOrder");

        migrationBuilder.DropColumn(
            name: "SAPGICPRNo",
            table: "SaleOrder");

        migrationBuilder.DropColumn(
            name: "SAPGICReservationNo",
            table: "SaleOrder");

        migrationBuilder.DropColumn(
            name: "SAPGICSalesPIC",
            table: "SaleOrder");

        migrationBuilder.DropColumn(
            name: "SAPGICSubAssetCode",
            table: "SaleOrder");
    }
}
