using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations
{
    /// <inheritdoc />
    public partial class Updated_PriceOffer_20250710_115700 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastApprovalRouteCreationTime",
                table: "PriceOffer",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastApprovalRouteCreatorId",
                table: "PriceOffer",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastApprovalRouteCreatorName",
                table: "PriceOffer",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastApprovalRouteCreatorUsername",
                table: "PriceOffer",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastApprovalRouteCreationTime",
                table: "PriceOffer");

            migrationBuilder.DropColumn(
                name: "LastApprovalRouteCreatorId",
                table: "PriceOffer");

            migrationBuilder.DropColumn(
                name: "LastApprovalRouteCreatorName",
                table: "PriceOffer");

            migrationBuilder.DropColumn(
                name: "LastApprovalRouteCreatorUsername",
                table: "PriceOffer");
        }
    }
}
