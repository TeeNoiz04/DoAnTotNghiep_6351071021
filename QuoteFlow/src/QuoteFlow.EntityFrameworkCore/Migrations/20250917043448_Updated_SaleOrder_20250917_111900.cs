using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteFlow.Migrations
{
    /// <inheritdoc />
    public partial class Updated_SaleOrder_20250917_111900 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GICProcess",
                table: "SaleOrder",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GICType",
                table: "SaleOrder",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SOType",
                table: "SaleOrder",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GICProcess",
                table: "SaleOrder");

            migrationBuilder.DropColumn(
                name: "GICType",
                table: "SaleOrder");

            migrationBuilder.DropColumn(
                name: "SOType",
                table: "SaleOrder");
        }
    }
}
