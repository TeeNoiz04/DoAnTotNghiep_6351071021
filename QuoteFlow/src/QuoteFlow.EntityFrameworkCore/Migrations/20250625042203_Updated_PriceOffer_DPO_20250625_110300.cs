using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_PriceOffer_DPO_20250625_110300 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<decimal>(
            name: "RequestedDiscountRatio",
            table: "PriceOfferDetail",
            type: "decimal(18,5)",
            precision: 18,
            scale: 5,
            nullable: true,
            oldClrType: typeof(decimal),
            oldType: "decimal(18,2)",
            oldPrecision: 18,
            oldScale: 2,
            oldNullable: true);

        migrationBuilder.AlterColumn<decimal>(
            name: "ActualDiscountRatio",
            table: "PriceOfferDetail",
            type: "decimal(18,5)",
            precision: 18,
            scale: 5,
            nullable: true,
            oldClrType: typeof(decimal),
            oldType: "decimal(18,2)",
            oldPrecision: 18,
            oldScale: 2,
            oldNullable: true);

        migrationBuilder.AlterColumn<decimal>(
            name: "SPO_DiscountRatio_CFG",
            table: "PriceOffer",
            type: "decimal(18,5)",
            precision: 18,
            scale: 5,
            nullable: true,
            oldClrType: typeof(decimal),
            oldType: "decimal(18,2)",
            oldPrecision: 18,
            oldScale: 2,
            oldNullable: true);

        migrationBuilder.AlterColumn<decimal>(
            name: "SPO_DiscountRatio",
            table: "PriceOffer",
            type: "decimal(18,5)",
            precision: 18,
            scale: 5,
            nullable: true,
            oldClrType: typeof(decimal),
            oldType: "decimal(18,2)",
            oldPrecision: 18,
            oldScale: 2,
            oldNullable: true);

        //migrationBuilder.AddColumn<string>(
        //    name: "SpecialInputPriceAccountName",
        //    table: "PriceOffer",
        //    type: "nvarchar(500)",
        //    maxLength: 500,
        //    nullable: true);

        //migrationBuilder.AddColumn<DateTime>(
        //    name: "SpecialInputPriceAssignedTime",
        //    table: "PriceOffer",
        //    type: "datetime2",
        //    nullable: true);

        //migrationBuilder.AddColumn<string>(
        //    name: "SpecialInputPriceAssignerFullName",
        //    table: "PriceOffer",
        //    type: "nvarchar(255)",
        //    maxLength: 255,
        //    nullable: true);

        //migrationBuilder.AddColumn<Guid>(
        //    name: "SpecialInputPriceAssignerId",
        //    table: "PriceOffer",
        //    type: "uniqueidentifier",
        //    nullable: true);

        //migrationBuilder.AddColumn<string>(
        //    name: "SpecialInputPriceAssignerUsername",
        //    table: "PriceOffer",
        //    type: "nvarchar(100)",
        //    maxLength: 100,
        //    nullable: true);

        //migrationBuilder.AddColumn<string>(
        //    name: "SpecialInputPriceAssignmentNote",
        //    table: "PriceOffer",
        //    type: "nvarchar(4000)",
        //    maxLength: 4000,
        //    nullable: true);

        //migrationBuilder.AddColumn<Guid>(
        //    name: "SpecialInputPriceId",
        //    table: "PriceOffer",
        //    type: "uniqueidentifier",
        //    nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "DeleterId",
            table: "DPODetail",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "DeleterName",
            table: "DPODetail",
            type: "nvarchar(255)",
            maxLength: 255,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "DeleterUsername",
            table: "DPODetail",
            type: "nvarchar(100)",
            maxLength: 100,
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "DeletionTime",
            table: "DPODetail",
            type: "datetime2",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "IsDeleted",
            table: "DPODetail",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<Guid>(
            name: "DeleterId",
            table: "DPO",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "DeleterName",
            table: "DPO",
            type: "nvarchar(255)",
            maxLength: 255,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "DeleterUsername",
            table: "DPO",
            type: "nvarchar(100)",
            maxLength: 100,
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "DeletionTime",
            table: "DPO",
            type: "datetime2",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "IsDeleted",
            table: "DPO",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AlterColumn<DateTime>(
            name: "CreationTime",
            table: "Attachments",
            type: "datetime2",
            nullable: true,
            oldClrType: typeof(DateTime),
            oldType: "datetime2");

        migrationBuilder.AlterColumn<string>(
            name: "AttachName",
            table: "Attachments",
            type: "nvarchar(500)",
            maxLength: 500,
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "nvarchar(500)",
            oldMaxLength: 500,
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "AttachCode",
            table: "Attachments",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(50)",
            oldMaxLength: 50);

        //migrationBuilder.AddColumn<string>(
        //    name: "CreatorName",
        //    table: "Attachments",
        //    type: "nvarchar(255)",
        //    maxLength: 255,
        //    nullable: true);

        //migrationBuilder.AddColumn<string>(
        //    name: "CreatorUsername",
        //    table: "Attachments",
        //    type: "nvarchar(100)",
        //    maxLength: 100,
        //    nullable: true);

        //migrationBuilder.AddColumn<string>(
        //    name: "LastModifierName",
        //    table: "Attachments",
        //    type: "nvarchar(255)",
        //    maxLength: 255,
        //    nullable: true);

        //migrationBuilder.AddColumn<string>(
        //    name: "LastModifierUsername",
        //    table: "Attachments",
        //    type: "nvarchar(100)",
        //    maxLength: 100,
        //    nullable: true);

        //migrationBuilder.CreateIndex(
        //    name: "IX_PriceOffer_SpecialInputPriceId",
        //    table: "PriceOffer",
        //    column: "SpecialInputPriceId");

        //migrationBuilder.AddForeignKey(
        //    name: "FK_PriceOffer_SpecialInputPrice_SpecialInputPriceId",
        //    table: "PriceOffer",
        //    column: "SpecialInputPriceId",
        //    principalTable: "SpecialInputPrice",
        //    principalColumn: "Id",
        //    onDelete: ReferentialAction.SetNull);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_PriceOffer_SpecialInputPrice_SpecialInputPriceId",
            table: "PriceOffer");

        migrationBuilder.DropIndex(
            name: "IX_PriceOffer_SpecialInputPriceId",
            table: "PriceOffer");

        migrationBuilder.DropColumn(
            name: "SpecialInputPriceAccountName",
            table: "PriceOffer");

        migrationBuilder.DropColumn(
            name: "SpecialInputPriceAssignedTime",
            table: "PriceOffer");

        migrationBuilder.DropColumn(
            name: "SpecialInputPriceAssignerFullName",
            table: "PriceOffer");

        migrationBuilder.DropColumn(
            name: "SpecialInputPriceAssignerId",
            table: "PriceOffer");

        migrationBuilder.DropColumn(
            name: "SpecialInputPriceAssignerUsername",
            table: "PriceOffer");

        migrationBuilder.DropColumn(
            name: "SpecialInputPriceAssignmentNote",
            table: "PriceOffer");

        migrationBuilder.DropColumn(
            name: "SpecialInputPriceId",
            table: "PriceOffer");

        migrationBuilder.DropColumn(
            name: "DeleterId",
            table: "DPODetail");

        migrationBuilder.DropColumn(
            name: "DeleterName",
            table: "DPODetail");

        migrationBuilder.DropColumn(
            name: "DeleterUsername",
            table: "DPODetail");

        migrationBuilder.DropColumn(
            name: "DeletionTime",
            table: "DPODetail");

        migrationBuilder.DropColumn(
            name: "IsDeleted",
            table: "DPODetail");

        migrationBuilder.DropColumn(
            name: "DeleterId",
            table: "DPO");

        migrationBuilder.DropColumn(
            name: "DeleterName",
            table: "DPO");

        migrationBuilder.DropColumn(
            name: "DeleterUsername",
            table: "DPO");

        migrationBuilder.DropColumn(
            name: "DeletionTime",
            table: "DPO");

        migrationBuilder.DropColumn(
            name: "IsDeleted",
            table: "DPO");

        migrationBuilder.DropColumn(
            name: "CreatorName",
            table: "Attachments");

        migrationBuilder.DropColumn(
            name: "CreatorUsername",
            table: "Attachments");

        migrationBuilder.DropColumn(
            name: "LastModifierName",
            table: "Attachments");

        migrationBuilder.DropColumn(
            name: "LastModifierUsername",
            table: "Attachments");

        migrationBuilder.AlterColumn<decimal>(
            name: "RequestedDiscountRatio",
            table: "PriceOfferDetail",
            type: "decimal(18,2)",
            precision: 18,
            scale: 2,
            nullable: true,
            oldClrType: typeof(decimal),
            oldType: "decimal(18,5)",
            oldPrecision: 18,
            oldScale: 5,
            oldNullable: true);

        migrationBuilder.AlterColumn<decimal>(
            name: "ActualDiscountRatio",
            table: "PriceOfferDetail",
            type: "decimal(18,2)",
            precision: 18,
            scale: 2,
            nullable: true,
            oldClrType: typeof(decimal),
            oldType: "decimal(18,5)",
            oldPrecision: 18,
            oldScale: 5,
            oldNullable: true);

        migrationBuilder.AlterColumn<decimal>(
            name: "SPO_DiscountRatio_CFG",
            table: "PriceOffer",
            type: "decimal(18,2)",
            precision: 18,
            scale: 2,
            nullable: true,
            oldClrType: typeof(decimal),
            oldType: "decimal(18,5)",
            oldPrecision: 18,
            oldScale: 5,
            oldNullable: true);

        migrationBuilder.AlterColumn<decimal>(
            name: "SPO_DiscountRatio",
            table: "PriceOffer",
            type: "decimal(18,2)",
            precision: 18,
            scale: 2,
            nullable: true,
            oldClrType: typeof(decimal),
            oldType: "decimal(18,5)",
            oldPrecision: 18,
            oldScale: 5,
            oldNullable: true);

        migrationBuilder.AlterColumn<DateTime>(
            name: "CreationTime",
            table: "Attachments",
            type: "datetime2",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
            oldClrType: typeof(DateTime),
            oldType: "datetime2",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "AttachName",
            table: "Attachments",
            type: "nvarchar(500)",
            maxLength: 500,
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(500)",
            oldMaxLength: 500);

        migrationBuilder.AlterColumn<string>(
            name: "AttachCode",
            table: "Attachments",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "nvarchar(50)",
            oldMaxLength: 50,
            oldNullable: true);
    }
}
