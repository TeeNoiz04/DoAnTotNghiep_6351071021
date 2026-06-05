using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_Discussions_20250702_110700 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "EntityType",
            table: "Discussion",
            type: "nvarchar(13)",
            maxLength: 13,
            nullable: false,
            defaultValue: "");

        migrationBuilder.AddColumn<Guid>(
            name: "PriceOfferId",
            table: "Discussion",
            type: "uniqueidentifier",
            nullable: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "EntityType",
            table: "Discussion");

        migrationBuilder.DropColumn(
            name: "PriceOfferId",
            table: "Discussion");
    }
}
