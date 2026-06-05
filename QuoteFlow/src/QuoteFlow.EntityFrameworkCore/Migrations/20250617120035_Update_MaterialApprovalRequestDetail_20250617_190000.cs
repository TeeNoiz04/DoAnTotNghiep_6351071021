using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Update_MaterialApprovalRequestDetail_20250617_190000 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<int>(
            name: "ReferenceLeadTime",
            table: "Materials",
            type: "int",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(400)",
            oldMaxLength: 400,
            oldNullable: true);

        migrationBuilder.AlterColumn<int>(
            name: "ReferenceLeadTime",
            table: "MaterialApprovalRequestDetail",
            type: "int",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "nvarchar(400)",
            oldMaxLength: 400,
            oldNullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "FinalDPOAcceptanceDate",
            table: "MaterialApprovalRequestDetail",
            type: "datetime2",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Reason",
            table: "MaterialApprovalRequestDetail",
            type: "nvarchar(4000)",
            maxLength: 4000,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Source",
            table: "MaterialApprovalRequestDetail",
            type: "nvarchar(4000)",
            maxLength: 4000,
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "FinalDPOAcceptanceDate",
            table: "MaterialApprovalRequestDetail");

        migrationBuilder.DropColumn(
            name: "Reason",
            table: "MaterialApprovalRequestDetail");

        migrationBuilder.DropColumn(
            name: "Source",
            table: "MaterialApprovalRequestDetail");

        migrationBuilder.AlterColumn<string>(
            name: "ReferenceLeadTime",
            table: "Materials",
            type: "nvarchar(400)",
            maxLength: 400,
            nullable: true,
            oldClrType: typeof(int),
            oldType: "int",
            oldNullable: true);

        migrationBuilder.AlterColumn<string>(
            name: "ReferenceLeadTime",
            table: "MaterialApprovalRequestDetail",
            type: "nvarchar(400)",
            maxLength: 400,
            nullable: true,
            oldClrType: typeof(int),
            oldType: "int",
            oldNullable: true);
    }
}
