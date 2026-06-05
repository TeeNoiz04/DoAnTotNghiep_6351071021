using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations
{
    /// <inheritdoc />
    public partial class Update_SaleOrderDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GICAssetClass",
                table: "SaleOrderDetail");

            migrationBuilder.DropColumn(
                name: "GICAssetName",
                table: "SaleOrderDetail");

            migrationBuilder.DropColumn(
                name: "GICMainAssetCode",
                table: "SaleOrderDetail");

            migrationBuilder.DropColumn(
                name: "GICSubAssetCode",
                table: "SaleOrderDetail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GICAssetClass",
                table: "SaleOrderDetail",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GICAssetName",
                table: "SaleOrderDetail",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GICMainAssetCode",
                table: "SaleOrderDetail",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GICSubAssetCode",
                table: "SaleOrderDetail",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);
        }
    }
}
