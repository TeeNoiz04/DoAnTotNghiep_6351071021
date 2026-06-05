using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_HistoryTracking_20250914_235200 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<DateTime>(
            name: "CreationTime",
            table: "HistoryTracking",
            type: "datetime2",
            nullable: true,
            oldClrType: typeof(DateTime),
            oldType: "datetime2");

        migrationBuilder.AddColumn<string>(
            name: "CreatorName",
            table: "HistoryTracking",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "CreatorUsername",
            table: "HistoryTracking",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "LastModifierName",
            table: "HistoryTracking",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "LastModifierUsername",
            table: "HistoryTracking",
            type: "nvarchar(max)",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "CreatorName",
            table: "HistoryTracking");

        migrationBuilder.DropColumn(
            name: "CreatorUsername",
            table: "HistoryTracking");

        migrationBuilder.DropColumn(
            name: "LastModifierName",
            table: "HistoryTracking");

        migrationBuilder.DropColumn(
            name: "LastModifierUsername",
            table: "HistoryTracking");

        migrationBuilder.AlterColumn<DateTime>(
            name: "CreationTime",
            table: "HistoryTracking",
            type: "datetime2",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
            oldClrType: typeof(DateTime),
            oldType: "datetime2",
            oldNullable: true);
    }
}
