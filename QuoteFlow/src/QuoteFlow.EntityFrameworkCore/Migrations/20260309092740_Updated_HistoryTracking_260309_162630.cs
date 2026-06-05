using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations
{
    /// <inheritdoc />
    public partial class Updated_HistoryTracking_260309_162630 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AssetId",
                table: "HistoryTracking",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HistoryTracking_AssetId",
                table: "HistoryTracking",
                column: "AssetId");

            migrationBuilder.AddForeignKey(
                name: "FK_HistoryTracking_Assets_AssetId",
                table: "HistoryTracking",
                column: "AssetId",
                principalTable: "Assets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HistoryTracking_Assets_AssetId",
                table: "HistoryTracking");

            migrationBuilder.DropIndex(
                name: "IX_HistoryTracking_AssetId",
                table: "HistoryTracking");

            migrationBuilder.DropColumn(
                name: "AssetId",
                table: "HistoryTracking");
        }
    }
}
