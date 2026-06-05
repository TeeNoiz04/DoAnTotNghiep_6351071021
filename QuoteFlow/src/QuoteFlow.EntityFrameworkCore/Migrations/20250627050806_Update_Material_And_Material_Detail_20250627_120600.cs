using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Update_Material_And_Material_Detail_20250627_120600 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_MaterialApprovalRequestDetail_SystemCategories_Material_Group",
            table: "MaterialApprovalRequestDetail");

        migrationBuilder.DropIndex(
            name: "IX_MaterialApprovalRequestDetail_Material_Group",
            table: "MaterialApprovalRequestDetail");

        migrationBuilder.DropColumn(
            name: "Description_Group",
            table: "Materials");

        migrationBuilder.DropColumn(
            name: "DestinationDate",
            table: "Materials");

        migrationBuilder.DropColumn(
            name: "Factory",
            table: "Materials");

        migrationBuilder.DropColumn(
            name: "FinalDPOAcceptanceDate",
            table: "Materials");

        migrationBuilder.DropColumn(
            name: "Kind",
            table: "Materials");

        migrationBuilder.DropColumn(
            name: "LeadTime",
            table: "Materials");

        migrationBuilder.DropColumn(
            name: "Origin",
            table: "Materials");

        migrationBuilder.DropColumn(
            name: "Reason",
            table: "Materials");

        migrationBuilder.DropColumn(
            name: "RefExchangeRate",
            table: "Materials");

        migrationBuilder.DropColumn(
            name: "Source",
            table: "Materials");

        migrationBuilder.DropColumn(
            name: "Vendor",
            table: "Materials");

        migrationBuilder.AlterColumn<string>(
            name: "Material_Group",
            table: "Materials",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "Material_Group",
            table: "MaterialApprovalRequestDetail",
            type: "nvarchar(max)",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldNullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Action",
            table: "MaterialApprovalRequestDetail",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "ActionDate",
            table: "MaterialApprovalRequestDetail",
            type: "datetime2",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "FactoryRefDoc",
            table: "MaterialApprovalRequestDetail",
            type: "nvarchar(max)",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Action",
            table: "MaterialApprovalRequestDetail");

        migrationBuilder.DropColumn(
            name: "ActionDate",
            table: "MaterialApprovalRequestDetail");

        migrationBuilder.DropColumn(
            name: "FactoryRefDoc",
            table: "MaterialApprovalRequestDetail");

        migrationBuilder.AlterColumn<Guid>(
            name: "Material_Group",
            table: "Materials",
            type: "uniqueidentifier",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(50)",
            oldMaxLength: 50,
            oldNullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Description_Group",
            table: "Materials",
            type: "nvarchar(4000)",
            maxLength: 4000,
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "DestinationDate",
            table: "Materials",
            type: "datetime2",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "Factory",
            table: "Materials",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "FinalDPOAcceptanceDate",
            table: "Materials",
            type: "datetime2",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Kind",
            table: "Materials",
            type: "nvarchar(20)",
            maxLength: 20,
            nullable: true);

        migrationBuilder.AddColumn<int>(
            name: "LeadTime",
            table: "Materials",
            type: "int",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Origin",
            table: "Materials",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Reason",
            table: "Materials",
            type: "nvarchar(4000)",
            maxLength: 4000,
            nullable: true);

        migrationBuilder.AddColumn<decimal>(
            name: "RefExchangeRate",
            table: "Materials",
            type: "decimal(18,2)",
            precision: 18,
            scale: 2,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Source",
            table: "Materials",
            type: "nvarchar(4000)",
            maxLength: 4000,
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "Vendor",
            table: "Materials",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AlterColumn<Guid>(
            name: "Material_Group",
            table: "MaterialApprovalRequestDetail",
            type: "uniqueidentifier",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)",
            oldNullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_MaterialApprovalRequestDetail_Material_Group",
            table: "MaterialApprovalRequestDetail",
            column: "Material_Group");

        migrationBuilder.AddForeignKey(
            name: "FK_MaterialApprovalRequestDetail_SystemCategories_Material_Group",
            table: "MaterialApprovalRequestDetail",
            column: "Material_Group",
            principalTable: "SystemCategories",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }
}
