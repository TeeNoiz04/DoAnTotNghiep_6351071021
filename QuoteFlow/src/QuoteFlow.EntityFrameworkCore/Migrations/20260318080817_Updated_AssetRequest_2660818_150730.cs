using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations
{
    /// <inheritdoc />
    public partial class Updated_AssetRequest_2660818_150730 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrentApprovalRound",
                table: "AssetRequest",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Lending_ActualReturnDate",
                table: "AssetRequest",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Lending_CustomerName",
                table: "AssetRequest",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentApprovalRound",
                table: "AssetRequest");

            migrationBuilder.DropColumn(
                name: "Lending_ActualReturnDate",
                table: "AssetRequest");

            migrationBuilder.DropColumn(
                name: "Lending_CustomerName",
                table: "AssetRequest");
        }
    }
}
