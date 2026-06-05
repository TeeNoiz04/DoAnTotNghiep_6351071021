using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations
{
    /// <inheritdoc />
    public partial class Updated_AssetRequest_20260307_040930 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PICDesc",
                table: "AssetRequestDetail");

            migrationBuilder.DropColumn(
                name: "PICSrc",
                table: "AssetRequestDetail");

            migrationBuilder.DropColumn(
                name: "WarehouseDestId",
                table: "AssetRequestDetail");

            migrationBuilder.DropColumn(
                name: "WarehouseDestName",
                table: "AssetRequestDetail");

            migrationBuilder.DropColumn(
                name: "WarehouseSrcId",
                table: "AssetRequestDetail");

            migrationBuilder.DropColumn(
                name: "WarehouseSrcName",
                table: "AssetRequestDetail");

            migrationBuilder.RenameColumn(
                name: "Lending_CompanyName",
                table: "AssetRequest",
                newName: "WarehouseSrcName");

            migrationBuilder.AlterColumn<string>(
                name: "AssetName",
                table: "AssetRequestDetail",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AssetRequestDetail",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Lending_CustomerTaxCode",
                table: "AssetRequest",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PIC_Dest",
                table: "AssetRequest",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RequestOwner",
                table: "AssetRequest",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WarehouseDestId",
                table: "AssetRequest",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WarehouseDestName",
                table: "AssetRequest",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WarehouseSrcId",
                table: "AssetRequest",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AssetRequestDetail_AssetId",
                table: "AssetRequestDetail",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_AssetRequestDetail_RequestId",
                table: "AssetRequestDetail",
                column: "RequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetRequestDetail_AssetRequest_RequestId",
                table: "AssetRequestDetail",
                column: "RequestId",
                principalTable: "AssetRequest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetRequestDetail_Assets_AssetId",
                table: "AssetRequestDetail",
                column: "AssetId",
                principalTable: "Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssetRequestDetail_AssetRequest_RequestId",
                table: "AssetRequestDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetRequestDetail_Assets_AssetId",
                table: "AssetRequestDetail");

            migrationBuilder.DropIndex(
                name: "IX_AssetRequestDetail_AssetId",
                table: "AssetRequestDetail");

            migrationBuilder.DropIndex(
                name: "IX_AssetRequestDetail_RequestId",
                table: "AssetRequestDetail");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AssetRequestDetail");

            migrationBuilder.DropColumn(
                name: "Lending_CustomerTaxCode",
                table: "AssetRequest");

            migrationBuilder.DropColumn(
                name: "PIC_Dest",
                table: "AssetRequest");

            migrationBuilder.DropColumn(
                name: "RequestOwner",
                table: "AssetRequest");

            migrationBuilder.DropColumn(
                name: "WarehouseDestId",
                table: "AssetRequest");

            migrationBuilder.DropColumn(
                name: "WarehouseDestName",
                table: "AssetRequest");

            migrationBuilder.DropColumn(
                name: "WarehouseSrcId",
                table: "AssetRequest");

            migrationBuilder.RenameColumn(
                name: "WarehouseSrcName",
                table: "AssetRequest",
                newName: "Lending_CompanyName");

            migrationBuilder.AlterColumn<string>(
                name: "AssetName",
                table: "AssetRequestDetail",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<string>(
                name: "PICDesc",
                table: "AssetRequestDetail",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PICSrc",
                table: "AssetRequestDetail",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WarehouseDestId",
                table: "AssetRequestDetail",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WarehouseDestName",
                table: "AssetRequestDetail",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WarehouseSrcId",
                table: "AssetRequestDetail",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WarehouseSrcName",
                table: "AssetRequestDetail",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }
    }
}
