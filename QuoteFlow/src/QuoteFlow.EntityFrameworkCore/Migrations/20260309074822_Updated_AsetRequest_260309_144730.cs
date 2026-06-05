using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations
{
    /// <inheritdoc />
    public partial class Updated_AsetRequest_260309_144730 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AssetRequestId",
                table: "ApprovalHistories",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalHistories_AssetRequestId",
                table: "ApprovalHistories",
                column: "AssetRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalHistories_AssetRequest_AssetRequestId",
                table: "ApprovalHistories",
                column: "AssetRequestId",
                principalTable: "AssetRequest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalHistories_AssetRequest_AssetRequestId",
                table: "ApprovalHistories");

            migrationBuilder.DropIndex(
                name: "IX_ApprovalHistories_AssetRequestId",
                table: "ApprovalHistories");

            migrationBuilder.DropColumn(
                name: "AssetRequestId",
                table: "ApprovalHistories");
        }
    }
}
