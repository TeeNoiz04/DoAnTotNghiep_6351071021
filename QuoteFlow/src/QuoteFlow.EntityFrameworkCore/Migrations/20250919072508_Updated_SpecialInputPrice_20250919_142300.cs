using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations
{
    /// <inheritdoc />
    public partial class Updated_SpecialInputPrice_20250919_142300 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                table: "SpecialInputPriceDetail",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeleterName",
                table: "SpecialInputPriceDetail",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeleterUsername",
                table: "SpecialInputPriceDetail",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "SpecialInputPriceDetail",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "SpecialInputPriceDetail",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                table: "SpecialInputPrice",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeleterName",
                table: "SpecialInputPrice",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeleterUsername",
                table: "SpecialInputPrice",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "SpecialInputPrice",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "SpecialInputPrice",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "SpecialInputPriceDetail");

            migrationBuilder.DropColumn(
                name: "DeleterName",
                table: "SpecialInputPriceDetail");

            migrationBuilder.DropColumn(
                name: "DeleterUsername",
                table: "SpecialInputPriceDetail");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "SpecialInputPriceDetail");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "SpecialInputPriceDetail");

            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "SpecialInputPrice");

            migrationBuilder.DropColumn(
                name: "DeleterName",
                table: "SpecialInputPrice");

            migrationBuilder.DropColumn(
                name: "DeleterUsername",
                table: "SpecialInputPrice");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "SpecialInputPrice");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "SpecialInputPrice");
        }
    }
}
