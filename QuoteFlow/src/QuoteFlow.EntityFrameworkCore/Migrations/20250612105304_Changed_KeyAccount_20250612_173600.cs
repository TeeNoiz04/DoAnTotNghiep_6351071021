using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Changed_KeyAccount_20250612_173600 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_KeyAccount_Buyer_DistributorId",
            table: "KeyAccount");

        migrationBuilder.DropColumn(
            name: "Email",
            table: "KeyAccount");

        migrationBuilder.DropColumn(
            name: "JobTitle",
            table: "KeyAccount");

        migrationBuilder.DropColumn(
            name: "LastPODate",
            table: "KeyAccount");

        migrationBuilder.DropColumn(
            name: "MEVNSalePIC",
            table: "KeyAccount");

        migrationBuilder.DropColumn(
            name: "NationalityId",
            table: "KeyAccount");

        migrationBuilder.DropColumn(
            name: "Province",
            table: "KeyAccount");

        migrationBuilder.DropColumn(
            name: "TaxCode",
            table: "KeyAccount");

        migrationBuilder.RenameColumn(
            name: "Website",
            table: "KeyAccount",
            newName: "TargetEndUser");

        migrationBuilder.RenameColumn(
            name: "TypeOfBusiness",
            table: "KeyAccount",
            newName: "Industry");

        migrationBuilder.RenameColumn(
            name: "TargetEndUsers",
            table: "KeyAccount",
            newName: "CustomerWebsite");

        migrationBuilder.RenameColumn(
            name: "TargetEU",
            table: "KeyAccount",
            newName: "CustomerPhone");

        migrationBuilder.RenameColumn(
            name: "RegisterName",
            table: "KeyAccount",
            newName: "CustomerProvince");

        migrationBuilder.RenameColumn(
            name: "Phone",
            table: "KeyAccount",
            newName: "CustomerCountry");

        migrationBuilder.RenameColumn(
            name: "PersonInCharge",
            table: "KeyAccount",
            newName: "BuyerShortName");

        migrationBuilder.RenameColumn(
            name: "DistributorId",
            table: "KeyAccount",
            newName: "BuyerId");

        migrationBuilder.RenameColumn(
            name: "CustomerFullName",
            table: "KeyAccount",
            newName: "CustomerName");

        migrationBuilder.RenameColumn(
            name: "Address",
            table: "KeyAccount",
            newName: "MaterialType");

        migrationBuilder.RenameIndex(
            name: "IX_KeyAccount_DistributorId",
            table: "KeyAccount",
            newName: "IX_KeyAccount_BuyerId");

        migrationBuilder.AlterColumn<string>(
            name: "KeyAccountShortName",
            table: "KeyAccount",
            type: "nvarchar(400)",
            maxLength: 400,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(400)",
            oldMaxLength: 400);

        migrationBuilder.AlterColumn<string>(
            name: "KeyAccountName",
            table: "KeyAccount",
            type: "nvarchar(400)",
            maxLength: 400,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(400)",
            oldMaxLength: 400);

        migrationBuilder.AlterColumn<Guid>(
            name: "KeyAccountClassId",
            table: "KeyAccount",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier",
            oldNullable: true);

        migrationBuilder.AddColumn<string>(
            name: "CustomerAddress",
            table: "KeyAccount",
            type: "nvarchar(500)",
            maxLength: 500,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "CustomerLocationDescription",
            table: "KeyAccount",
            type: "nvarchar(500)",
            maxLength: 500,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "CustomerTaxCode",
            table: "KeyAccount",
            type: "nvarchar(4000)",
            maxLength: 4000,
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<bool>(
            name: "IsDeactive",
            table: "KeyAccount",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<string>(
            name: "KeyAccountClassDescription",
            table: "KeyAccount",
            type: "nvarchar(500)",
            maxLength: 500,
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<string>(
            name: "KeyAccountTypeDescription",
            table: "KeyAccount",
            type: "nvarchar(500)",
            maxLength: 500,
            nullable: false,
            defaultValue: "");

        migrationBuilder.CreateIndex(
            name: "IX_KeyAccount_CustomerLocationId",
            table: "KeyAccount",
            column: "CustomerLocationId");

        migrationBuilder.AddForeignKey(
            name: "FK_KeyAccount_Distributors_BuyerId",
            table: "KeyAccount",
            column: "BuyerId",
            principalTable: "Distributors",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_KeyAccount_SystemCategories_CustomerLocationId",
            table: "KeyAccount",
            column: "CustomerLocationId",
            principalTable: "SystemCategories",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_KeyAccount_Distributors_BuyerId",
            table: "KeyAccount");

        migrationBuilder.DropForeignKey(
            name: "FK_KeyAccount_SystemCategories_CustomerLocationId",
            table: "KeyAccount");

        migrationBuilder.DropIndex(
            name: "IX_KeyAccount_CustomerLocationId",
            table: "KeyAccount");

        migrationBuilder.DropColumn(
            name: "CustomerAddress",
            table: "KeyAccount");

        migrationBuilder.DropColumn(
            name: "CustomerLocationDescription",
            table: "KeyAccount");

        migrationBuilder.DropColumn(
            name: "CustomerTaxCode",
            table: "KeyAccount");

        migrationBuilder.DropColumn(
            name: "IsDeactive",
            table: "KeyAccount");

        migrationBuilder.DropColumn(
            name: "KeyAccountClassDescription",
            table: "KeyAccount");

        migrationBuilder.DropColumn(
            name: "KeyAccountTypeDescription",
            table: "KeyAccount");

        migrationBuilder.RenameColumn(
            name: "TargetEndUser",
            table: "KeyAccount",
            newName: "Website");

        migrationBuilder.RenameColumn(
            name: "MaterialType",
            table: "KeyAccount",
            newName: "Address");

        migrationBuilder.RenameColumn(
            name: "Industry",
            table: "KeyAccount",
            newName: "TypeOfBusiness");

        migrationBuilder.RenameColumn(
            name: "CustomerWebsite",
            table: "KeyAccount",
            newName: "TargetEndUsers");

        migrationBuilder.RenameColumn(
            name: "CustomerProvince",
            table: "KeyAccount",
            newName: "RegisterName");

        migrationBuilder.RenameColumn(
            name: "CustomerPhone",
            table: "KeyAccount",
            newName: "TargetEU");

        migrationBuilder.RenameColumn(
            name: "CustomerName",
            table: "KeyAccount",
            newName: "CustomerFullName");

        migrationBuilder.RenameColumn(
            name: "CustomerCountry",
            table: "KeyAccount",
            newName: "Phone");

        migrationBuilder.RenameColumn(
            name: "BuyerShortName",
            table: "KeyAccount",
            newName: "PersonInCharge");

        migrationBuilder.RenameColumn(
            name: "BuyerId",
            table: "KeyAccount",
            newName: "DistributorId");

        migrationBuilder.RenameIndex(
            name: "IX_KeyAccount_BuyerId",
            table: "KeyAccount",
            newName: "IX_KeyAccount_DistributorId");

        migrationBuilder.AlterColumn<string>(
            name: "KeyAccountShortName",
            table: "KeyAccount",
            type: "nvarchar(400)",
            maxLength: 400,
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "nvarchar(400)",
            oldMaxLength: 400,
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "KeyAccountName",
            table: "KeyAccount",
            type: "nvarchar(400)",
            maxLength: 400,
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "nvarchar(400)",
            oldMaxLength: 400,
            oldNullable: true);

        migrationBuilder.AlterColumn<Guid>(
            name: "KeyAccountClassId",
            table: "KeyAccount",
            type: "uniqueidentifier",
            nullable: true,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier");

        migrationBuilder.AddColumn<string>(
            name: "Email",
            table: "KeyAccount",
            type: "nvarchar(4000)",
            maxLength: 4000,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "JobTitle",
            table: "KeyAccount",
            type: "nvarchar(4000)",
            maxLength: 4000,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "LastPODate",
            table: "KeyAccount",
            type: "nvarchar(4000)",
            maxLength: 4000,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "MEVNSalePIC",
            table: "KeyAccount",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<Guid>(
            name: "NationalityId",
            table: "KeyAccount",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Province",
            table: "KeyAccount",
            type: "nvarchar(100)",
            maxLength: 100,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "TaxCode",
            table: "KeyAccount",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddForeignKey(
            name: "FK_KeyAccount_Buyer_DistributorId",
            table: "KeyAccount",
            column: "DistributorId",
            principalTable: "Buyer",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }
}
