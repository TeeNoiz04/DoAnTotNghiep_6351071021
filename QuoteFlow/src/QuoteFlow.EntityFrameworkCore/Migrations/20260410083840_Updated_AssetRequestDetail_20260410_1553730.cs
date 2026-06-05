using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations
{
    /// <inheritdoc />
    public partial class Updated_AssetRequestDetail_20260410_1553730 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "AssetRequestDetail",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AssetClass",
                table: "AssetRequestDetail",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AssetNote",
                table: "AssetRequestDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AssetType",
                table: "AssetRequestDetail",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodeMain",
                table: "AssetRequestDetail",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodeMain_AF",
                table: "AssetRequestDetail",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodeSub",
                table: "AssetRequestDetail",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CodeSub_AF",
                table: "AssetRequestDetail",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "AssetRequestDetail",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AssetRequestDetail",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Division",
                table: "AssetRequestDetail",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GIV",
                table: "AssetRequestDetail",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "InvoicePrice",
                table: "AssetRequestDetail",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaterialCode",
                table: "AssetRequestDetail",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModelName",
                table: "AssetRequestDetail",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfComponent",
                table: "AssetRequestDetail",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "POR",
                table: "AssetRequestDetail",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PR",
                table: "AssetRequestDetail",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "AssetRequestDetail",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Qty",
                table: "AssetRequestDetail",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "REG",
                table: "AssetRequestDetail",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SalePIC",
                table: "AssetRequestDetail",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Section",
                table: "AssetRequestDetail",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SectionSAP",
                table: "AssetRequestDetail",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "AssetRequestDetail",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "AssetRequestDetail",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WarehouseId",
                table: "AssetRequestDetail",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WarehouseName",
                table: "AssetRequestDetail",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "AssetRequestDetail");

            migrationBuilder.DropColumn(
                name: "AssetClass",
                table: "AssetRequestDetail");

            migrationBuilder.DropColumn(
                name: "AssetNote",
                table: "AssetRequestDetail");

            migrationBuilder.DropColumn(
                name: "AssetType",
                table: "AssetRequestDetail");

            migrationBuilder.DropColumn(
                name: "CodeMain",
                table: "AssetRequestDetail");

            migrationBuilder.DropColumn(
                name: "CodeMain_AF",
                table: "AssetRequestDetail");

            migrationBuilder.DropColumn(
                name: "CodeSub",
                table: "AssetRequestDetail");

            migrationBuilder.DropColumn(
                name: "CodeSub_AF",
                table: "AssetRequestDetail");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "AssetRequestDetail");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AssetRequestDetail");

            migrationBuilder.DropColumn(
                name: "Division",
                table: "AssetRequestDetail");

            migrationBuilder.DropColumn(
                name: "GIV",
                table: "AssetRequestDetail");

            migrationBuilder.DropColumn(
                name: "InvoicePrice",
                table: "AssetRequestDetail");

            migrationBuilder.DropColumn(
                name: "MaterialCode",
                table: "AssetRequestDetail");

            migrationBuilder.DropColumn(
                name: "ModelName",
                table: "AssetRequestDetail");

            migrationBuilder.DropColumn(
                name: "NumberOfComponent",
                table: "AssetRequestDetail");

            migrationBuilder.DropColumn(
                name: "POR",
                table: "AssetRequestDetail");

            migrationBuilder.DropColumn(
                name: "PR",
                table: "AssetRequestDetail");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "AssetRequestDetail");

            migrationBuilder.DropColumn(
                name: "Qty",
                table: "AssetRequestDetail");

            migrationBuilder.DropColumn(
                name: "REG",
                table: "AssetRequestDetail");

            migrationBuilder.DropColumn(
                name: "SalePIC",
                table: "AssetRequestDetail");

            migrationBuilder.DropColumn(
                name: "Section",
                table: "AssetRequestDetail");

            migrationBuilder.DropColumn(
                name: "SectionSAP",
                table: "AssetRequestDetail");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "AssetRequestDetail");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "AssetRequestDetail");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "AssetRequestDetail");

            migrationBuilder.DropColumn(
                name: "WarehouseName",
                table: "AssetRequestDetail");
        }
    }
}
