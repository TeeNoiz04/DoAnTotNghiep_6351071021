using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations
{
    /// <inheritdoc />
    public partial class Updated_DPO_DPODetail_20251024_114700 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Gkr_DpoUsed",
                table: "DPODetail",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Gkr_LinkedDpoId",
                table: "DPO",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gkr_LinkedDpoNo",
                table: "DPO",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gkr_DpoUsed",
                table: "DPODetail");

            migrationBuilder.DropColumn(
                name: "Gkr_LinkedDpoId",
                table: "DPO");

            migrationBuilder.DropColumn(
                name: "Gkr_LinkedDpoNo",
                table: "DPO");
        }
    }
}
