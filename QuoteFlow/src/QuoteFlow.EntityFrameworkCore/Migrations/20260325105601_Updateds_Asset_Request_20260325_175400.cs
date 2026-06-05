using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations
{
    /// <inheritdoc />
    public partial class Updateds_Asset_Request_20260325_175400 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Audit_FromDate",
                table: "AssetRequest",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Audit_ToDate",
                table: "AssetRequest",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtensionDoc",
                table: "AssetRequest",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Lending_Target",
                table: "AssetRequest",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Audit_FromDate",
                table: "AssetRequest");

            migrationBuilder.DropColumn(
                name: "Audit_ToDate",
                table: "AssetRequest");

            migrationBuilder.DropColumn(
                name: "ExtensionDoc",
                table: "AssetRequest");

            migrationBuilder.DropColumn(
                name: "Lending_Target",
                table: "AssetRequest");
        }
    }
}
