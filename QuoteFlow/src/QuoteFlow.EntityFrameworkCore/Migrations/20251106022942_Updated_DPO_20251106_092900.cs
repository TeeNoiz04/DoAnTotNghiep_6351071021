using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations
{
    /// <inheritdoc />
    public partial class Updated_DPO_20251106_092900 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Disposed",
                table: "SaleOrdersSAPImport",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Disposed",
                table: "SaleOrderDetail",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gkr_Reason",
                table: "DPO",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gkr_SalePicFullName",
                table: "DPO",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Gkr_SalePicTeamId",
                table: "DPO",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gkr_SalePicUsername",
                table: "DPO",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Disposed",
                table: "SaleOrdersSAPImport");

            migrationBuilder.DropColumn(
                name: "Disposed",
                table: "SaleOrderDetail");

            migrationBuilder.DropColumn(
                name: "Gkr_Reason",
                table: "DPO");

            migrationBuilder.DropColumn(
                name: "Gkr_SalePicFullName",
                table: "DPO");

            migrationBuilder.DropColumn(
                name: "Gkr_SalePicTeamId",
                table: "DPO");

            migrationBuilder.DropColumn(
                name: "Gkr_SalePicUsername",
                table: "DPO");
        }
    }
}
