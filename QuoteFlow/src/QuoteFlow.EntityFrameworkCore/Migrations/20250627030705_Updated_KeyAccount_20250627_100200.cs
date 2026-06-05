using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_KeyAccount_20250627_100200 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_KeyAccount_SystemCategories_CustomerLocationId",
            table: "KeyAccount");

        migrationBuilder.DropForeignKey(
            name: "FK_KeyAccount_SystemCategories_KeyAccountClassId",
            table: "KeyAccount");

        migrationBuilder.DropForeignKey(
            name: "FK_KeyAccount_SystemCategories_KeyAccountTypeId",
            table: "KeyAccount");

        migrationBuilder.DropIndex(
            name: "IX_KeyAccount_CustomerLocationId",
            table: "KeyAccount");

        migrationBuilder.DropIndex(
            name: "IX_KeyAccount_KeyAccountClassId",
            table: "KeyAccount");

        migrationBuilder.DropIndex(
            name: "IX_KeyAccount_KeyAccountTypeId",
            table: "KeyAccount");

        migrationBuilder.DropColumn(
            name: "CustomerLocationId",
            table: "KeyAccount");

        migrationBuilder.DropColumn(
            name: "KeyAccountClassDescription",
            table: "KeyAccount");

        migrationBuilder.DropColumn(
            name: "KeyAccountClassId",
            table: "KeyAccount");

        migrationBuilder.DropColumn(
            name: "KeyAccountTypeDescription",
            table: "KeyAccount");

        migrationBuilder.DropColumn(
            name: "KeyAccountTypeId",
            table: "KeyAccount");

        migrationBuilder.RenameColumn(
            name: "CustomerLocationDescription",
            table: "KeyAccount",
            newName: "KeyAccountType");

        migrationBuilder.AddColumn<string>(
            name: "CustomerLocation",
            table: "KeyAccount",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "KeyAccountClass",
            table: "KeyAccount",
            type: "nvarchar(500)",
            maxLength: 500,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "KeyAccountClassBuyer",
            table: "KeyAccount",
            type: "nvarchar(500)",
            maxLength: 500,
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "CustomerLocation",
            table: "KeyAccount");

        migrationBuilder.DropColumn(
            name: "KeyAccountClass",
            table: "KeyAccount");

        migrationBuilder.DropColumn(
            name: "KeyAccountClassBuyer",
            table: "KeyAccount");

        migrationBuilder.RenameColumn(
            name: "KeyAccountType",
            table: "KeyAccount",
            newName: "CustomerLocationDescription");

        migrationBuilder.AddColumn<Guid>(
            name: "CustomerLocationId",
            table: "KeyAccount",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.AddColumn<string>(
            name: "KeyAccountClassDescription",
            table: "KeyAccount",
            type: "nvarchar(500)",
            maxLength: 500,
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<Guid>(
            name: "KeyAccountClassId",
            table: "KeyAccount",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.AddColumn<string>(
            name: "KeyAccountTypeDescription",
            table: "KeyAccount",
            type: "nvarchar(500)",
            maxLength: 500,
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<Guid>(
            name: "KeyAccountTypeId",
            table: "KeyAccount",
            type: "uniqueidentifier",
            nullable: false,
            defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

        migrationBuilder.CreateIndex(
            name: "IX_KeyAccount_CustomerLocationId",
            table: "KeyAccount",
            column: "CustomerLocationId");

        migrationBuilder.CreateIndex(
            name: "IX_KeyAccount_KeyAccountClassId",
            table: "KeyAccount",
            column: "KeyAccountClassId");

        migrationBuilder.CreateIndex(
            name: "IX_KeyAccount_KeyAccountTypeId",
            table: "KeyAccount",
            column: "KeyAccountTypeId");

        migrationBuilder.AddForeignKey(
            name: "FK_KeyAccount_SystemCategories_CustomerLocationId",
            table: "KeyAccount",
            column: "CustomerLocationId",
            principalTable: "SystemCategories",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_KeyAccount_SystemCategories_KeyAccountClassId",
            table: "KeyAccount",
            column: "KeyAccountClassId",
            principalTable: "SystemCategories",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);

        migrationBuilder.AddForeignKey(
            name: "FK_KeyAccount_SystemCategories_KeyAccountTypeId",
            table: "KeyAccount",
            column: "KeyAccountTypeId",
            principalTable: "SystemCategories",
            principalColumn: "Id",
            onDelete: ReferentialAction.Restrict);
    }
}
