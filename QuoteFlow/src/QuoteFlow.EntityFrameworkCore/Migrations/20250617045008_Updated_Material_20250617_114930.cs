using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_Material_20250617_114930 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<DateTime>(
            name: "FinalDPOAcceptanceDate",
            table: "Materials",
            type: "datetime2",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Reason",
            table: "Materials",
            type: "nvarchar(4000)",
            maxLength: 4000,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "Source",
            table: "Materials",
            type: "nvarchar(4000)",
            maxLength: 4000,
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "FinalDPOAcceptanceDate",
            table: "Materials");

        migrationBuilder.DropColumn(
            name: "Reason",
            table: "Materials");

        migrationBuilder.DropColumn(
            name: "Source",
            table: "Materials");
    }
}
