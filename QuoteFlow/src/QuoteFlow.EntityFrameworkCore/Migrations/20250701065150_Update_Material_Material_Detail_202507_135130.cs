using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Update_Material_Material_Detail_202507_135130 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_MaterialApprovalRequestDetail_SystemCategories_InputCurrency",
            table: "MaterialApprovalRequestDetail");

        migrationBuilder.DropIndex(
            name: "IX_MaterialApprovalRequestDetail_InputCurrency",
            table: "MaterialApprovalRequestDetail");

        migrationBuilder.AlterColumn<string>(
            name: "InputCurrency",
            table: "Materials",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "InputCurrency",
            table: "MaterialApprovalRequestDetail",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldNullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_PSI_Detail_PSI_Id",
            table: "PSI_Detail",
            column: "PSI_Id");

        migrationBuilder.CreateIndex(
            name: "IX_CargoData_CargoId",
            table: "CargoData",
            column: "CargoId");

        migrationBuilder.AddForeignKey(
            name: "FK_CargoData_Cargo_CargoId",
            table: "CargoData",
            column: "CargoId",
            principalTable: "Cargo",
            principalColumn: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_PSI_Detail_PSI_PSI_Id",
            table: "PSI_Detail",
            column: "PSI_Id",
            principalTable: "PSI",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_CargoData_Cargo_CargoId",
            table: "CargoData");

        migrationBuilder.DropForeignKey(
            name: "FK_PSI_Detail_PSI_PSI_Id",
            table: "PSI_Detail");

        migrationBuilder.DropIndex(
            name: "IX_PSI_Detail_PSI_Id",
            table: "PSI_Detail");

        migrationBuilder.DropIndex(
            name: "IX_CargoData_CargoId",
            table: "CargoData");

        migrationBuilder.AlterColumn<Guid>(
            name: "InputCurrency",
            table: "Materials",
            type: "uniqueidentifier",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(50)",
            oldMaxLength: 50,
            oldNullable: true);

        migrationBuilder.AlterColumn<Guid>(
            name: "InputCurrency",
            table: "MaterialApprovalRequestDetail",
            type: "uniqueidentifier",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(50)",
            oldMaxLength: 50,
            oldNullable: true);

        migrationBuilder.CreateIndex(
            name: "IX_MaterialApprovalRequestDetail_InputCurrency",
            table: "MaterialApprovalRequestDetail",
            column: "InputCurrency");

        migrationBuilder.AddForeignKey(
            name: "FK_MaterialApprovalRequestDetail_SystemCategories_InputCurrency",
            table: "MaterialApprovalRequestDetail",
            column: "InputCurrency",
            principalTable: "SystemCategories",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }
}
