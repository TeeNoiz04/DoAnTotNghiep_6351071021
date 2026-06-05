using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations
{
    /// <inheritdoc />
    public partial class Updated_ApprovalRoute_260309_112100 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastApprovalRouteCreationTime",
                table: "AssetRequest",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastApprovalRouteCreatorId",
                table: "AssetRequest",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastApprovalRouteCreatorName",
                table: "AssetRequest",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastApprovalRouteCreatorUsername",
                table: "AssetRequest",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AssetRequestId",
                table: "ApprovalRoute",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalRoute_AssetRequestId",
                table: "ApprovalRoute",
                column: "AssetRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalRoute_AssetRequest_AssetRequestId",
                table: "ApprovalRoute",
                column: "AssetRequestId",
                principalTable: "AssetRequest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalRoute_AssetRequest_AssetRequestId",
                table: "ApprovalRoute");

            migrationBuilder.DropIndex(
                name: "IX_ApprovalRoute_AssetRequestId",
                table: "ApprovalRoute");

            migrationBuilder.DropColumn(
                name: "LastApprovalRouteCreationTime",
                table: "AssetRequest");

            migrationBuilder.DropColumn(
                name: "LastApprovalRouteCreatorId",
                table: "AssetRequest");

            migrationBuilder.DropColumn(
                name: "LastApprovalRouteCreatorName",
                table: "AssetRequest");

            migrationBuilder.DropColumn(
                name: "LastApprovalRouteCreatorUsername",
                table: "AssetRequest");

            migrationBuilder.DropColumn(
                name: "AssetRequestId",
                table: "ApprovalRoute");
        }
    }
}
