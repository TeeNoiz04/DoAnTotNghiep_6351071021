using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Update_Material_And_Material_Detail_20250627_151430 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "Description_Group",
            table: "MaterialApprovalRequestDetail");

        migrationBuilder.DropColumn(
            name: "DestinationDate",
            table: "MaterialApprovalRequestDetail");

        migrationBuilder.DropColumn(
            name: "Factory",
            table: "MaterialApprovalRequestDetail");

        migrationBuilder.DropColumn(
            name: "Kind",
            table: "MaterialApprovalRequestDetail");

        migrationBuilder.DropColumn(
            name: "LeadTime",
            table: "MaterialApprovalRequestDetail");

        migrationBuilder.DropColumn(
            name: "Origin",
            table: "MaterialApprovalRequestDetail");

        migrationBuilder.DropColumn(
            name: "RefExchangeRate",
            table: "MaterialApprovalRequestDetail");

        migrationBuilder.DropColumn(
            name: "Vendor",
            table: "MaterialApprovalRequestDetail");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Description_Group",
            table: "MaterialApprovalRequestDetail",
            type: "nvarchar(4000)",
            maxLength: 4000,
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "DestinationDate",
            table: "MaterialApprovalRequestDetail",
            type: "datetime2",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "Factory",
            table: "MaterialApprovalRequestDetail",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Kind",
            table: "MaterialApprovalRequestDetail",
            type: "nvarchar(20)",
            maxLength: 20,
            nullable: true);

        migrationBuilder.AddColumn<int>(
            name: "LeadTime",
            table: "MaterialApprovalRequestDetail",
            type: "int",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Origin",
            table: "MaterialApprovalRequestDetail",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true);

        migrationBuilder.AddColumn<decimal>(
            name: "RefExchangeRate",
            table: "MaterialApprovalRequestDetail",
            type: "decimal(18,2)",
            nullable: true);

        migrationBuilder.AddColumn<Guid>(
            name: "Vendor",
            table: "MaterialApprovalRequestDetail",
            type: "uniqueidentifier",
            nullable: true);
    }
}
