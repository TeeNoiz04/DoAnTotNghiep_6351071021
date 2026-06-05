using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace QuoteFlow.Migrations
{
    /// <inheritdoc />
    public partial class Updated_SaleOrder_20251222_104530 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<string>(
            //    name: "StockCodeConfirmed",
            //    table: "StockImport",
            //    type: "nvarchar(50)",
            //    maxLength: 50,
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "StockNameConfirmed",
            //    table: "StockImport",
            //    type: "nvarchar(500)",
            //    maxLength: 500,
            //    nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SOId",
                table: "ApprovalHistories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalHistories_SOId",
                table: "ApprovalHistories",
                column: "SOId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalHistories_SaleOrder_SOId",
                table: "ApprovalHistories",
                column: "SOId",
                principalTable: "SaleOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalHistories_SaleOrder_SOId",
                table: "ApprovalHistories");

            migrationBuilder.DropIndex(
                name: "IX_ApprovalHistories_SOId",
                table: "ApprovalHistories");

            //migrationBuilder.DropColumn(
            //    name: "StockCodeConfirmed",
            //    table: "StockImport");

            //migrationBuilder.DropColumn(
            //    name: "StockNameConfirmed",
            //    table: "StockImport");

            migrationBuilder.DropColumn(
                name: "SOId",
                table: "ApprovalHistories");
        }
    }
}
