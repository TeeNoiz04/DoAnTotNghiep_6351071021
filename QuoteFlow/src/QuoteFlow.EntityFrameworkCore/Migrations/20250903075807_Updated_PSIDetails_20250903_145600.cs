using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_PSIDetails_20250903_145600 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "MaterialGroupNo",
            table: "PSI_Detail");

        migrationBuilder.AddColumn<Guid>(
            name: "DeleterId",
            table: "PSI_Detail",
            type: "uniqueidentifier",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "DeleterName",
            table: "PSI_Detail",
            type: "nvarchar(255)",
            maxLength: 255,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "DeleterUsername",
            table: "PSI_Detail",
            type: "nvarchar(100)",
            maxLength: 100,
            nullable: true);

        migrationBuilder.AddColumn<DateTime>(
            name: "DeletionTime",
            table: "PSI_Detail",
            type: "datetime2",
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "IsDeleted",
            table: "PSI_Detail",
            type: "bit",
            nullable: false,
            defaultValue: false);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "DeleterId",
            table: "PSI_Detail");

        migrationBuilder.DropColumn(
            name: "DeleterName",
            table: "PSI_Detail");

        migrationBuilder.DropColumn(
            name: "DeleterUsername",
            table: "PSI_Detail");

        migrationBuilder.DropColumn(
            name: "DeletionTime",
            table: "PSI_Detail");

        migrationBuilder.DropColumn(
            name: "IsDeleted",
            table: "PSI_Detail");

        migrationBuilder.AddColumn<int>(
            name: "MaterialGroupNo",
            table: "PSI_Detail",
            type: "int",
            nullable: true);
    }
}
