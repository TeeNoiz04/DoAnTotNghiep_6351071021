using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations;

/// <inheritdoc />
public partial class Updated_SO_20250717_132600 : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {


        migrationBuilder.AddColumn<bool>(
            name: "IsDeleted",
            table: "SaleOrderDetail",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<bool>(
            name: "IsDeleted",
            table: "SaleOrder",
            type: "bit",
            nullable: false,
            defaultValue: false);

    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {


        migrationBuilder.DropColumn(
            name: "IsDeleted",
            table: "SaleOrderDetail");

        migrationBuilder.DropColumn(
            name: "IsDeleted",
            table: "SaleOrder");


    }
}
