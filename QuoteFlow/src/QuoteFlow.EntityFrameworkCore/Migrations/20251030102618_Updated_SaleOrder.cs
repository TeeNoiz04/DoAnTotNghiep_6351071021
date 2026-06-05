using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations
{
    /// <inheritdoc />
    public partial class Updated_SaleOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.RenameColumn(
                name: "SAPGICSubAssetCode",
                table: "SaleOrder",
                newName: "GICGivNo");

            migrationBuilder.RenameColumn(
                name: "SAPGICGivDate",
                table: "SaleOrder",
                newName: "GICGivDate");

            migrationBuilder.AddColumn<string>(
                name: "ChangeNote",
                table: "SaleOrderDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GICAssetClass",
                table: "SaleOrderDetail",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GICAssetName",
                table: "SaleOrderDetail",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "GICGivDate",
                table: "SaleOrderDetail",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GICGivNo",
                table: "SaleOrderDetail",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GICLocation",
                table: "SaleOrderDetail",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GICMainAssetCode",
                table: "SaleOrderDetail",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GICPorNo",
                table: "SaleOrderDetail",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GICPrNo",
                table: "SaleOrderDetail",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GICReservationNo",
                table: "SaleOrderDetail",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GICSalePIC",
                table: "SaleOrderDetail",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GICSubAssetCode",
                table: "SaleOrderDetail",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SAPAmountLandingCost",
                table: "SaleOrderDetail",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SAPLandingCost",
                table: "SaleOrderDetail",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "CompletelyClosed",
                table: "SaleOrder",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChangeNote",
                table: "SaleOrderDetail");

            migrationBuilder.DropColumn(
                name: "GICAssetClass",
                table: "SaleOrderDetail");

            migrationBuilder.DropColumn(
                name: "GICAssetName",
                table: "SaleOrderDetail");

            migrationBuilder.DropColumn(
                name: "GICGivDate",
                table: "SaleOrderDetail");

            migrationBuilder.DropColumn(
                name: "GICGivNo",
                table: "SaleOrderDetail");

            migrationBuilder.DropColumn(
                name: "GICLocation",
                table: "SaleOrderDetail");

            migrationBuilder.DropColumn(
                name: "GICMainAssetCode",
                table: "SaleOrderDetail");

            migrationBuilder.DropColumn(
                name: "GICPorNo",
                table: "SaleOrderDetail");

            migrationBuilder.DropColumn(
                name: "GICPrNo",
                table: "SaleOrderDetail");

            migrationBuilder.DropColumn(
                name: "GICReservationNo",
                table: "SaleOrderDetail");

            migrationBuilder.DropColumn(
                name: "GICSalePIC",
                table: "SaleOrderDetail");

            migrationBuilder.DropColumn(
                name: "GICSubAssetCode",
                table: "SaleOrderDetail");

            migrationBuilder.DropColumn(
                name: "SAPAmountLandingCost",
                table: "SaleOrderDetail");

            migrationBuilder.DropColumn(
                name: "SAPLandingCost",
                table: "SaleOrderDetail");

            migrationBuilder.DropColumn(
                name: "CompletelyClosed",
                table: "SaleOrder");

            migrationBuilder.RenameColumn(
                name: "GICGivNo",
                table: "SaleOrder",
                newName: "SAPGICSubAssetCode");

            migrationBuilder.RenameColumn(
                name: "GICGivDate",
                table: "SaleOrder",
                newName: "SAPGICGivDate");

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
        }
    }
}
